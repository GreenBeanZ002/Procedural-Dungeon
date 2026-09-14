using System.Collections.Generic;
using UnityEngine;

public static class MonsterUtils
{
    public static void DrawMonsters(IEnumerable<Vector2Int> positions, GameObject monsterPrefab)
    {
        foreach (var pos in positions)
        {
            Debug.Log("Generating Object");

            if (Random.Range(0f, 100f) < 2)
            {
                int roll = Random.Range(0, 100);
                if (roll < 50)
                {
                    Object.Instantiate(monsterPrefab, new Vector3(pos.x + 0.5f, pos.y + 0.5f, -0.9f), Quaternion.identity);
                    Debug.Log("Drawing Skeleton");
                }
            }
        }
    }

}