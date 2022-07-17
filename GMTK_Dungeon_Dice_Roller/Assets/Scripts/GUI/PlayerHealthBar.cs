using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    public int PlayerHealth = 5;
    public GameObject healthPointPrefab;

    private void Awake()
    {
        int currentHealthPoints = transform.childCount;
        while (currentHealthPoints < PlayerHealth)
        {
            GameObject newInstance = Instantiate(healthPointPrefab, transform);
            newInstance.GetComponent<HealthTweenAnimations>().Show();
            currentHealthPoints = transform.childCount;
        }

    }
    
    void Start() {
        LevelManager.OnChangeHealth += health => SetPlayerHealth(health);
    }

    public void SetPlayerHealth(int newHealth)
    {
        if (newHealth < PlayerHealth)
        {
            RemovePlayerHealth(PlayerHealth-newHealth);
        }
        else
        {
            AddPlayerHealth(newHealth-PlayerHealth);
        }
    }

    public void RemovePlayerHealth(int health)
    {
        if (PlayerHealth == 0) return;

        int healthToRemove = health;
        if (PlayerHealth - health <= 0) // Make sure we don't remove more health than we currently have
        {
            healthToRemove = PlayerHealth; // This would kill the player
        }

        int numOfChildren = transform.childCount;
        for (int i = 0; i < healthToRemove; i++)
        { 
            transform.GetChild(numOfChildren - i - 1).GetComponent<HealthTweenAnimations>().Hide();
        }

        PlayerHealth -= healthToRemove;
    }

    public void AddPlayerHealth(int health)
    {
        for (int i = 0; i < health; i++)
        {
            GameObject newInstance = Instantiate(healthPointPrefab, transform);
            newInstance.GetComponent<HealthTweenAnimations>().Show();
        }
        
        PlayerHealth += health;
    }
}
