using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    public int PlayerHealth = 5;
    public GameObject healthPointPrefab;
    
    void Start() {
        int currentHealthPoints = transform.childCount;
        while (currentHealthPoints < PlayerHealth)
        {
            GameObject newInstance = Instantiate(healthPointPrefab, transform);
            newInstance.GetComponent<HealthTweenAnimations>().Show();
            currentHealthPoints = transform.childCount;
        }
        LevelManager.OnChangeHealth += LevelManagerOnOnChangeHealth();
    }

    private LevelManager.ModifierIntCallback LevelManagerOnOnChangeHealth()
    {
        return health => SetPlayerHealth(health);
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
        int removedHealth = 0;
        int i = 0;
        while (removedHealth < healthToRemove)
        {
            GameObject obj = transform.GetChild(numOfChildren - i - 1).gameObject;
            i++;
            if (LeanTween.isTweening(obj)) 
            {
                continue;
            }
            obj.GetComponent<HealthTweenAnimations>().Hide();
            removedHealth++;
        }

       
        PlayerHealth -= healthToRemove;
    }

    public void AddPlayerHealth(int health)
    {
        Debug.Log(transform);
         int numOfChildren = transform.childCount;
         if (numOfChildren > PlayerHealth) {
            for (int i = PlayerHealth; i < numOfChildren; i++) {
                GameObject obj = transform.GetChild(i).gameObject;
                LeanTween.cancel(obj);
                obj.GetComponent<HealthTweenAnimations>().Show();
                health--;
            }
         }

        for (int i = 0; i < health; i++)
        {
            GameObject newInstance = Instantiate(healthPointPrefab, transform);
            newInstance.GetComponent<HealthTweenAnimations>().Show();
        }
        
        PlayerHealth += health;
    }

    private void OnDestroy()
    {
        LevelManager.OnChangeHealth -= LevelManagerOnOnChangeHealth();
    }
}
