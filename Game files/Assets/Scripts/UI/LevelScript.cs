using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUtils : MonoBehaviour
{
    public static int exp;

    private int level;

    public static int getExp() { return exp; }
    public static int getLevel(int exp)    {        float tempLevel = Mathf.Sqrt(exp / 50f);        return Mathf.FloorToInt(tempLevel);    }

    public static int getExpToLevel(int level){        return 50 * (level * level);    }

    public static void addExp(int amount) { exp += amount; }
    public static int getExpToNextLevel(int exp, bool returnAsPercentage) 
    { 
        int currentLevel = getLevel(exp); 
        int nextLevelExp = getExpToLevel(currentLevel+1);
        int expToLevel = getExpToLevel(getLevel(nextLevelExp));
        int alreadyusedExp = exp - getExpToLevel(currentLevel);

        if (returnAsPercentage) 
        {
            int currentExp = alreadyusedExp;
            return Mathf.FloorToInt((currentExp / (float)expToLevel) * 100);
        }
        else
        {
            return expToLevel - alreadyusedExp;
        }
    }



    public static void AddExp(int amtToAdd)    {        exp += amtToAdd;    }

    public static void ResetExp() { exp = 0; }
}
