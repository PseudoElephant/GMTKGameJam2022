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

    private void Awake()
    {
        // Getting the first binding of the input action using index of 0. If we had more bindings, we would use different indices.
        buttonControl = (ButtonControl)shootAction.controls[0];

        LevelManager.OnPlayerFire += () => Debug.Log("Hello");
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
                GameObject bigB = Instantiate(bigBullet, transform.position + new Vector3(dir.x, dir.y, 0) * offset, Quaternion.identity);
                bigB.GetComponent<Bullet>().SetDirection(dir);
                return;
            }

            AudioManager.Play("sfx_shoot_soft");
            GameObject smallA = Instantiate(smallBullet, transform.position +  new Vector3(dir.x, dir.y, 0) * offset, Quaternion.identity);
            smallA.GetComponent<Bullet>().SetDirection(dir);
        }
    }
}
