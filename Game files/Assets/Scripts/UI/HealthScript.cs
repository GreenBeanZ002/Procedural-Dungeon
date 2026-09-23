using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public static int health = 100;

    public static int maxHealth = 100;
    
    static public int GetHealth() { return health; }

    static public int GetMaxHealth() { return maxHealth; }
    static public void AddHealth(int amtToAdd) { health += amtToAdd; }

    static public void RemoveHealth(int amtToRemove) { 

        if(health - amtToRemove <= 0)
        {
            health = 0;
            Die();
        }
        else
        {
            health -= amtToRemove;
        }
         
    
    }

    static public void AddMaxHealth(int amtToAdd) { maxHealth += amtToAdd; }

    static public void RemoveMaxHealth(int amtToRemove) { maxHealth -= amtToRemove; }

    static public void Die() 
    {
        coinsManager.Instance.resetCoins();
        LevelUtils.ResetExp();
        health = maxHealth;
    }
}
