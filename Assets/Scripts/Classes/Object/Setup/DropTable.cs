using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;

[System.Serializable]
public class DropEntry
{
    public Item itemPrefab;
    [Range(0, 100)] public float dropChance = 100f;
    public int minAmount = 1;
    public int maxAmount = 1;
}
public class DropTable : MonoBehaviour
{
    [SerializeField]
    private List<DropEntry> drops;

    [SerializeField]
    private float scatterRadius = 0.5f;

    public void DropAll(Vector3 position)
    {
        foreach (DropEntry entry in drops)
        {
            if (entry.itemPrefab == null) continue;

            if (Rand.Range(0f, 100f) <= entry.dropChance)
            {
                int amount = Rand.Range(entry.minAmount, entry.maxAmount + 1); // +1 since max is exclusive

                for (int i = 0; i < amount; i++)
                {
                    Vector3 offset = new Vector3(
                        Rand.Range(-scatterRadius, scatterRadius),
                        Rand.Range(-scatterRadius, scatterRadius),
                        0f
                    );

                    Vector3 spawnPos = position + offset;
                    spawnPos.z = -0.4f;
                    Instantiate(entry.itemPrefab,spawnPos, Quaternion.identity);
                }
            }
        }
    }
}
