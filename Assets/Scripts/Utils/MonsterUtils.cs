using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;

public class MonsterUtils
{
    public static HashSet<Vector2Int> monsterPos = new HashSet<Vector2Int>();
    public static void DrawMonsters(IEnumerable<Vector2Int> positions, List<MonsterEntry> monsterEntries)
    {
        foreach (var pos in positions)
        {
            Debug.Log("Generating Monster");

            if (Rand.Range(0f, 1000f) < 5)
            {
                monsterPos.Add(pos);

                GameObject chosen = PickWeightedMonster(monsterEntries);

                if (chosen != null)
                {
                    GameObject.Instantiate(chosen, new Vector3(pos.x + 0.5f, pos.y + 0.5f, -0.9f), Quaternion.identity);
                    Debug.Log($"Drawing {chosen.name}");
                }
            }
        }
    }
    private static GameObject PickWeightedMonster(List<MonsterEntry> monsters)
    {
        float totalWeight = 0f;
        foreach (var entry in monsters)
            totalWeight += entry.spawnChance;

        if (totalWeight <= 0f) return null;

        float roll = Rand.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var entry in monsters)
        {
            cumulative += entry.spawnChance;
            if (roll < cumulative)
            {
                return entry.monsterPrefab;
            }
        }

        return null;
    }

}