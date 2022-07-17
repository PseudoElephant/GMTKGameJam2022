using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

[RequireComponent(typeof(Controller))]
public class Player : MonoBehaviour
{

    public int health = 10;
    void Start() {
        Debug.Log("Player Start");
        LevelManager.SetHealth(health);
    }

    private void OnDeath()
    {
        LeanTween.scale(gameObject, Vector3.zero, 1f).setEaseInOutSine().setOnComplete(() => Destroy(gameObject));
        LeanTween.rotateAround(gameObject, Vector3.forward, 360, 1f).setEaseInOutSine();
        LevelManager.BroadcastEvent(LevelManager.Event.PlayerDeath);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "HarmPlayer") return;
        health -= col.gameObject.GetComponent<Bullet>().damage;
        LevelManager.SetHealth(health);
        if (health <= 0)
        {
            OnDeath();
        }
    }

}
