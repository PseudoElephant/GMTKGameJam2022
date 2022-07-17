using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice
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

    public static void SetGoodDice() {
        int itemCount = System.Enum.GetValues(typeof(Dice.GoodDice)).Length;
        System.Random rnd = new System.Random();
        LevelManager.AddGoodDice((Dice.GoodDice)rnd.Next(itemCount));
    }

    public static void SetBadDice() {
        int itemCount = System.Enum.GetValues(typeof(Dice.BadDice)).Length;
        System.Random rnd = new System.Random();
        LevelManager.AddBadDice((Dice.BadDice)rnd.Next(itemCount));
    }

}
