using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DoorsTween))]
public static class Dice : MonoBehaviour
{

    public enum GoodDice {
        ExtraLife,
        IncreaseSpeed,
        IncreaseDamage,
        LaserBeam,
        ExtraShot,
        IncreaseAttackSpeed,
        FasterBulletSpeed,
        NukeGame,
        LowerDashCooldown
    }

    public enum BadDice {
        FasterEnemies,
        SpawnEnemy,
        NoDash,
        SpawnBomb,
        DuplicateEnemies,
        IncreaseEnemyHealth,
        IncreaseEnemyDamage,
    }

    public static Dice.GoodDice GetGoodDice() {
        int itemCount = System.Enum.GetValues(typeof(Dice.GoodDice)).Length;
        System.Random rnd = new System.Random();
        LevelManager.AddGoodDice(rnd.Next(itemCount));
    }

    public static Dice.BadDice GetBadDice() {
        int itemCount = System.Enum.GetValues(typeof(Dice.BadDice)).Length;
        System.Random rnd = new System.Random();
        LevelManager.AddBadDice(rnd.Next(itemCount));
    }

}
