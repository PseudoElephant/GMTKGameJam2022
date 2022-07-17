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

    public struct EntityBuff {
       public int speed;
       public int shootSpeed;
       public int damage;
       public int health;
    }

    public struct PlayerBuff {
        public int speed;
        public float shootSpeed;
        public int damage;
        public int health;
        public float dashCooldown;
        public int extraShots;
    }

    private EntityBuff enemyBuff;
    private PlayerBuff playerBuff;

    private int _enemyCount;    

    public enum Event 
    {
    PlayerHit,
    PlayerFire,
    DiceRoll,
    EnemyKilled,
    PlayerDeath,
    ExtraLife,
    GoodRoll,
    BadRoll
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

    public static event ModifierIntCallback OnIncreaseDamage;
    public static event CallbackAction OnDuplicate;
    public static event ModifierCallback OnDashCountChange;
    public static event ModifierCallback OnIncreaseEnemyHealth;
    public static event ModifierCallback OnIncreaseEnemyDamage;
    public static event ModifierCallback OnIncreaseEnemySpeed;

    public static event CallbackAction OnGoodDiceRoll;
    public static event CallbackAction OnBadDiceRoll;


    private void Awake() {
        Debug.Log("Waking LevelManger");
        if (Instance == null) {
            Instance = this;
            enemyBuff = new EntityBuff();
            playerBuff = new PlayerBuff();
            playerBuff.health = 3;
            
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
        if (OnEnemyKilled != null)
        {
            LevelManager.OnEnemyKilled();
        }
        
        if (LevelBeaten()){
            if (OnLevelBeaten != null)
            {
                LevelManager.OnLevelBeaten();
            }

            if (OnLevelFinish != null)
            {
                LevelManager.OnLevelFinish();
            }
        }
    }

    private static void LevelEnd() {
        OnPlayerDeath?.Invoke();
        OnLevelFinish?.Invoke();
    }

// Event Management
// BroadcastEvent triggers specific event.
    public static void BroadcastEvent(Event e) {
        switch(e) {
            case Event.PlayerHit:
            {
                LevelManager.OnPlayerHit?.Invoke();
                break;
            }
            case Event.PlayerFire:
            {
                LevelManager.OnPlayerFire?.Invoke();    
                break;
            }   
            case Event.DiceRoll:
            {
                LevelManager.OnDiceRoll?.Invoke();
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
                LevelManager.OnExtraLife?.Invoke();
                break;
            }
            case Event.GoodRoll:
                LevelManager.OnGoodDiceRoll?.Invoke();
                break;
            case Event.BadRoll:
                LevelManager.OnBadDiceRoll?.Invoke();
                break;
            default: break;
        }
    }
    // public static void BroadcastEvent(Event e, params string[] list) {
    
    public static void SetHealth(int health) {
        LevelManager.OnChangeHealth(health);
        LevelManager.Instance.playerBuff.health = health;
    }
    public static void AddGoodDice(Dice.GoodDice dice) {
    switch (dice) {
        case Dice.GoodDice.ExtraLife: 
        {
            LevelManager.BroadcastEvent(Event.ExtraLife);
            LevelManager.Instance.playerBuff.health++;
            break;
        }
        case Dice.GoodDice.IncreaseSpeed: 
        {
            OnIncreaseSpeed?.Invoke(1f);
            LevelManager.Instance.playerBuff.speed++;
            break;
        }
        case Dice.GoodDice.IncreaseDamage:
        {
            OnIncreaseDamage?.Invoke(5);
            LevelManager.Instance.playerBuff.damage+=5;
            break;
        }
        case Dice.GoodDice.ExtraShot:
        {
            OnExtraShot?.Invoke();
            LevelManager.Instance.playerBuff.extraShots++;
            break;
        }
        case Dice.GoodDice.FasterBulletSpeed:
        {
            OnIncreaseBulletSpeed?.Invoke(.1f);
            LevelManager.Instance.playerBuff.shootSpeed+=-.1f;
            break;
        }
        case Dice.GoodDice.LowerDashCooldown:
        {
            OnDecreaseDashCooldown?.Invoke(0.1f);
            LevelManager.Instance.playerBuff.dashCooldown+=0.1f;
            break;
        }

        default: break;
        }
    
        AudioManager.Play("sfx_ApplyGoodAbility");
    }

    public static void AddBadDice(Dice.BadDice dice) {
    
        switch (dice) {
        // could refactor to only send action, values and use in other comps
        case Dice.BadDice.FasterEnemies: 
        {
            OnIncreaseEnemySpeed?.Invoke(1f);
            LevelManager.Instance.enemyBuff.speed++;
            break;
        }
        case Dice.BadDice.SpawnEnemy: 
        {
            // Spawn Enemy
            break;
        }
        case Dice.BadDice.NoDash:
        {
            OnDashCountChange?.Invoke(0);
            break;
        }
        case Dice.BadDice.SpawnBomb:
        {
            // Spawn bad bomb
            break;
        }
        case Dice.BadDice.DuplicateEnemies:
        {
            OnDuplicate?.Invoke();
            break;
        }
        case Dice.BadDice.IncreaseEnemyHealth:
        {
            OnIncreaseEnemyHealth?.Invoke(10f);
            LevelManager.Instance.enemyBuff.health+=10;
            break;
        }
        case Dice.BadDice.IncreaseEnemyDamage:
        {
            OnIncreaseEnemyDamage?.Invoke(10f);
            LevelManager.Instance.enemyBuff.damage+=10;
            break;
        }
        default: break;
        }
    
        AudioManager.Play("sfx_ApplyGoodAbility");
    }

    public static EntityBuff GetEnemyBuffs() {
        return LevelManager.Instance.enemyBuff;
    }

     public static PlayerBuff GetPlayerBuffs() {
        return LevelManager.Instance.playerBuff;
     }

    
    IEnumerator LateStart() {
        // Triggers start scripts on all subscribers
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Late Start");
        LevelManager.OnLevelStart();
        if (LevelBeaten())
        {
            LevelManager.OnLevelBeaten?.Invoke();
        }
    }
}
