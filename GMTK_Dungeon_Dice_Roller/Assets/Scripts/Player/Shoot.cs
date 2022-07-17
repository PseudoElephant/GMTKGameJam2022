using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Shoot : MonoBehaviour
{
    [Header("Controls")] 
    [Tooltip("Configure your inputs")]
    public float timeForLongTap = 1f;
    public InputAction shootAction;
    private ButtonControl buttonControl;

    [Header("Bullets")] 
    [Tooltip("Bullets data")]
    public float offset = 5f;
    public GameObject smallBullet;
     public GameObject bigBullet;

    private float _startTime;

    private bool _fire;
    private bool _isStrongShoot;

    private int _damageAmp;

    private int _speedAmp;

    private int _extraShots = 3;

    private void Awake()
    {
        // Getting the first binding of the input action using index of 0. If we had more bindings, we would use different indices.
        buttonControl = (ButtonControl)shootAction.controls[0];
    }

    private void Start() {
    {
         LevelManager.PlayerBuff buffs = LevelManager.GetPlayerBuffs();
         timeForLongTap = Math.Clamp(timeForLongTap - buffs.shootSpeed, 0, 100);
         _damageAmp = Math.Clamp(_damageAmp + buffs.damage, 1, 10000);
         _extraShots += buffs.extraShots; 

         LevelManager.OnIncreaseBulletSpeed += (value) => {
            timeForLongTap = Math.Clamp(timeForLongTap - value, 0, 100);
         };

        LevelManager.OnIncreaseDamage += (value) => {
            _damageAmp += value;
         };

         
        LevelManager.OnExtraShot += () => {
            _extraShots += 1;
         };

    }
    }

    private void OnEnable() {
        shootAction.Enable();
    }

    private void OnDisable() {
        shootAction.Disable();
    }

    void Update() {
         if (buttonControl.wasPressedThisFrame) {
            Debug.Log("Was Just Pressed");
            _startTime = Time.time;
         }

        if (buttonControl.wasReleasedThisFrame) {
            Debug.Log("Was Just Released");
            float timePressed = Time.time - _startTime;
            _fire = true;
            _isStrongShoot = timePressed > timeForLongTap;
        }
    }

    void FixedUpdate() {
        if (_fire) {
            Debug.Log("Player Fire");
            _fire = false;
            LevelManager.BroadcastEvent(LevelManager.Event.PlayerFire);
        
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 dir = worldPos - (Vector2)GetComponent<Controller>().gameObject.transform.position;
            dir.Normalize();

            if (_isStrongShoot) {
                AudioManager.Play("sfx_shoot_hard");

                for (int i = -1; i < _extraShots; i++) {
                    Vector2 shootDir = new Vector2((float) Math.Cos((2*Math.PI/(10))*i), (float) Math.Sin((2*Math.PI/(10))*i)) * dir;
             
                    GameObject bigB = Instantiate(bigBullet, transform.position + new Vector3(shootDir.x, shootDir.y, 0) * offset, Quaternion.identity);
                    Bullet b = bigB.GetComponent<Bullet>();
                    b.SetDirection(dir);
                    b.damage += _damageAmp; 
                }

                return;
            }

            AudioManager.Play("sfx_shoot_soft");

            for (int i = 0; i < _extraShots + 1; i++) {
                Vector2 shootDir = new Vector2((float) Math.Cos((2*Math.PI/(10))*i), (float) Math.Sin((2*Math.PI/(10))*i)) * dir;

                GameObject smallA = Instantiate(smallBullet, transform.position + new Vector3(shootDir.x, shootDir.y, 0) * offset, Quaternion.identity);
                Bullet a = smallA.GetComponent<Bullet>();
                a.SetDirection(dir);
                a.damage += _damageAmp;
            }

        }
    }
}
