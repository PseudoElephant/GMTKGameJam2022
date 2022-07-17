using System;
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

    public static GoodDice SetGoodDice() {
        int itemCount = System.Enum.GetValues(typeof(Dice.GoodDice)).Length;
        System.Random rnd = new System.Random();
        
        Dice.GoodDice currentDice = (Dice.GoodDice)rnd.Next(itemCount);
        LevelManager.AddGoodDice(currentDice);
        return currentDice;
    }

    public static BadDice SetBadDice() {
        int itemCount = System.Enum.GetValues(typeof(Dice.BadDice)).Length;
        System.Random rnd = new System.Random();
        
        Dice.BadDice currentDice = (Dice.BadDice)rnd.Next(itemCount);
        LevelManager.AddBadDice(currentDice);
        return currentDice;
    }

    public static string GetGoodDiceString(GoodDice dice)
    {
        return dice switch
        {
            GoodDice.ExtraLife => "Extra Life!",
            GoodDice.IncreaseSpeed => "Speed Increased",
            GoodDice.IncreaseDamage => "Damage Increase",
            GoodDice.LaserBeam => "Laser Beam!",
            GoodDice.ExtraShot => "Extra Shot!",
            GoodDice.IncreaseAttackSpeed => "Increased Attack Speed",
            GoodDice.FasterBulletSpeed => "Faster Bullet Speed",
            GoodDice.NukeGame => "Nuke Em'!",
            GoodDice.LowerDashCooldown => "Faster Dash Cooldown",
            _ => throw new ArgumentOutOfRangeException(nameof(dice), dice, null)
        };
    }

    public static string GetBadDiceString(BadDice dice)
    {
        return dice switch
        {
            BadDice.FasterEnemies => "Faster Enemies",
            BadDice.SpawnEnemy => "Spawn Random Enemy",
            BadDice.NoDash => "No More Dash",
            BadDice.SpawnBomb => "Spawn Bomb!",
            BadDice.DuplicateEnemies => "Doppelganger Time",
            BadDice.IncreaseEnemyHealth => "Increased Enemy Health",
            BadDice.IncreaseEnemyDamage => "Increased Enemy Damage",
            _ => throw new ArgumentOutOfRangeException(nameof(dice), dice, null)
        };
    }

}
