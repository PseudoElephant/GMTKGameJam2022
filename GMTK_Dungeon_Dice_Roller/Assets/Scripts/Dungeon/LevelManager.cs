using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    // LevelManager Instance of the current room
    public static LevelManager Instance;
    // 
    public delegate void CallbackAction();

     public delegate void ModifierCallback(float value);

      public delegate void ModifierIntCallback(int value);

    public delegate void ItemCallback(GameObject value);

    private int _enemyCount;    

    public enum Event 
    {
    PlayerHit,
    PlayerFire,
    DiceRoll,
    EnemyKilled,
    PlayerDeath,
    ExtraLife
    }

// Level Events
// OnLevelStart Event Trigger when new room loads in
    public static event CallbackAction OnLevelStart;
// OnLevelBeaten Event Trigger when player kills all enemies
     public static event CallbackAction OnLevelBeaten;
// OnLevelFinish Trigger when players kills all enemies or dies
    public static event CallbackAction OnLevelFinish;
// OnPlayerDeath Trigger when player dies
    public static event CallbackAction OnPlayerDeath;
// OnPlayerHit Trigger when player gets hit
    public static event CallbackAction OnPlayerHit;
// OnPlayerFire Trigger when player fires
    public static event CallbackAction OnPlayerFire;
// OnDiceRoll Trigger on dice roll
    public static event CallbackAction OnDiceRoll;
// OnEnemyKilled Trigger on enemy killed
    public static event CallbackAction OnEnemyKilled;

// Dices
    public static event CallbackAction OnExtraLife;
    public static event CallbackAction OnExtraShot;
    public static event ModifierIntCallback OnChangeHealth;
    public static event ModifierCallback OnIncreaseBulletSpeed;

    public static event ModifierCallback OnIncreaseSpeed;

    public static event ModifierCallback OnDecreaseDashCooldown;

    public static event ModifierCallback OnIncreaseRate;

    public static event ModifierCallback OnIncreaseDamage;

    public static event ItemCallback OnNewItem;
    public static event CallbackAction OnDuplicate;
    public static event ModifierCallback OnDashCountChange;
    public static event ModifierCallback OnIncreaseEnemyHealth;
    public static event ModifierCallback OnIncreaseEnemyDamage;
     public static event ModifierCallback OnIncreaseEnemySpeed;


    private void Awake() {
        Debug.Log("Waking LevelManger");
        if (Instance == null) {
            Instance = this;
            return;
        }

        Destroy(this.gameObject);
    }

    private void Start() {
         StartCoroutine(LateStart());
    }

// Enemy Control 
    private bool LevelBeaten() {
        return _enemyCount <= 0;
    }

    public void AddEnemy() {
        _enemyCount++;
    }

    private void RemoveEnemy() {
        _enemyCount--;
        OnEnemyKilled();
        if (LevelBeaten()){
            OnLevelBeaten();
            OnLevelFinish();
        }
    }

    private static void LevelEnd() {
        OnPlayerDeath();
        OnLevelFinish();
    }

// Event Management
// BroadcastEvent triggers specific event.
    public static void BroadcastEvent(Event e) {
        switch(e) {
            case Event.PlayerHit:
            {
                LevelManager.OnPlayerHit();
                break;
            }
            case Event.PlayerFire:
            {
                LevelManager.OnPlayerFire();    
                break;
            }   
            case Event.DiceRoll:
            {
                LevelManager.OnDiceRoll();
                break;
            }   
            case Event.EnemyKilled:
            {
                LevelManager.Instance.RemoveEnemy();
                break;
            }
            case Event.PlayerDeath:
            {
                LevelManager.LevelEnd();
                break;
            }
            case Event.ExtraLife:
            {
                LevelManager.OnExtraLife();
                break;
            }
            default: break;
        }
    }
    // public static void BroadcastEvent(Event e, params string[] list) {
    
    public static void SetHealth(int health) {
        Debug.Log("Health Change");
        LevelManager.OnChangeHealth(health);
    }
    public static void AddGoodDice(Dice.GoodDice dice) {
    switch (dice) {
        case Dice.GoodDice.ExtraLife: 
        {
            LevelManager.BroadcastEvent(Event.ExtraLife);
            break;
        }
        case Dice.GoodDice.IncreaseSpeed: 
        {
            OnIncreaseSpeed(1f);
            break;
        }
        case Dice.GoodDice.IncreaseDamage:
        {
            OnIncreaseDamage(1f);
            break;
        }
        case Dice.GoodDice.LaserBeam:
        {
            // OnNewItem(prefab)
            break;
        }
        case Dice.GoodDice.ExtraShot:
        {
            OnExtraShot();
            break;
        }
        case Dice.GoodDice.IncreaseAttackSpeed:
        {
            OnIncreaseRate(1f);
            break;
        }
        case Dice.GoodDice.FasterBulletSpeed:
        {
            OnIncreaseBulletSpeed(1f);
            break;
        }
        case Dice.GoodDice.NukeGame:
        {
            // Spawn Bomb
            break;
        }
        case Dice.GoodDice.LowerDashCooldown:
        {
            OnDecreaseDashCooldown(1f);
            break;
        }

        default: break;
        }
    }

    public static void AddBadDice(Dice.BadDice dice) {
    
    switch (dice) {
        // could refactor to only send action, values and use in other comps
        case Dice.BadDice.FasterEnemies: 
        {
            OnIncreaseEnemySpeed(1f);
            break;
        }
        case Dice.BadDice.SpawnEnemy: 
        {
            // Spawn Enemy
            break;
        }
        case Dice.BadDice.NoDash:
        {
            OnDashCountChange(0);
            break;
        }
        case Dice.BadDice.SpawnBomb:
        {
            // Spawn bad bomb
            break;
        }
        case Dice.BadDice.DuplicateEnemies:
        {
            OnDuplicate();
            break;
        }
        case Dice.BadDice.IncreaseEnemyHealth:
        {
            OnIncreaseEnemyHealth(10f);
            break;
        }
        case Dice.BadDice.IncreaseEnemyDamage:
        {
            OnIncreaseEnemyDamage(10f);
            break;
        }
        default: break;
        }
    }
    // }
    IEnumerator LateStart() {
        // Triggers start scripts on all subscribers
        yield return new WaitForFixedUpdate();
        Debug.Log("Late Start");

        if (LevelBeaten()) {
            Debug.Log("Level Beaten");
        } else {
            LevelManager.OnLevelStart();
        }       
    }

}
