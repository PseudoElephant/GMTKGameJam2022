using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

[RequireComponent(typeof(Controller))]
public class Player : MonoBehaviour
{

    public int health = 10;
    private bool _isDying;
    void Start() {
        Debug.Log("Player Start");
        LevelManager.PlayerBuff buffs = LevelManager.GetPlayerBuffs();
        health = Math.Clamp(health + buffs.health, 1, 1000);
        LevelManager.SetHealth(health);  
        LevelManager.OnExtraLife += () => {
            health++; 
            LevelManager.SetHealth(health);
        };
    }
    
    private void OnDeath()
    {
        if (_isDying) return;
        _isDying = true;
        
        LeanTween.scale(gameObject, Vector3.zero, 1f).setEaseInOutSine().setOnComplete(() => Destroy(gameObject));
        LeanTween.rotateAround(gameObject, Vector3.forward, 360, 1f).setEaseInOutSine();
        LevelManager.BroadcastEvent(LevelManager.Event.PlayerDeath);
    }
    
    private void OnHit()
    {
        LevelManager.BroadcastEvent(LevelManager.Event.GoodRoll);
        LevelManager.BroadcastEvent(LevelManager.Event.DiceRoll);
        
        Vector3 originalScale = transform.localScale;
        LeanTween.scale(gameObject, originalScale + new Vector3(0.05f, 0.05f, 0.05f), 0.1f).setEaseInOutSine()
            .setOnComplete(
                () =>
                {
                    LeanTween.scale(gameObject, originalScale, 0.1f).setEaseInOutSine();
                });
        LeanTween.rotateLocal(gameObject, new Vector3(0f, 0f, 0.4f), 0.1f).setEaseInOutSine().setOnComplete(() =>
        {
            LeanTween.rotateLocal(gameObject, Vector3.zero, 0.1f).setEaseInOutSine();
        });
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "HarmPlayer") return;
        LevelManager.BroadcastEvent(LevelManager.Event.PlayerHit);
        health -= col.gameObject.GetComponent<Bullet>().damage;
        
        LevelManager.SetHealth(health);
        if (health <= 0)
        {
            OnDeath();
        }
        
        OnHit();
    }

}
