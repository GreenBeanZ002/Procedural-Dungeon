using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;

public class ObjectUtils :MonoBehaviour
{
    public static HashSet<Vector2Int> objectPos = new HashSet<Vector2Int>();

    public static HashSet<Vector2Int> getObjectPositions() { return objectPos; }
    public void drawObjects(IEnumerable<Vector2Int> positions, Player player, List<ObjectEntry> objects)
    {
        foreach (var pos in positions)
        {
            Debug.Log("Generating Object");

            if (Rand.Range(0f, 100f) < 10)
            {
                objectPos.Add(pos);

                WorldObject chosen = PickWeightedObject(objects);

                if (chosen != null)
                {
                    Instantiate(chosen, new Vector3(pos.x + 0.5f, pos.y + 0.5f, -0.9f), Quaternion.identity);
                    Debug.Log($"Drawing {chosen.name}");
                }
                else
                {
                    player.movePlayerToPos(new Vector3(pos.x + 0.5f, pos.y + 0.5f, -1));
                }
            }
        }
    }

    private WorldObject PickWeightedObject(List<ObjectEntry> objects)
    {
        float totalWeight = 0f;
        foreach (var entry in objects)
            totalWeight += entry.spawnChance;

        if (totalWeight <= 0f) return null;

        float roll = Rand.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var entry in objects)
        {
            cumulative += entry.spawnChance;
            if (roll < cumulative)
            {
                return entry.objectPrefab;
            }
        }

        return null;
    }
    //Object Item Drop function

    //Object break sound thing
}







