using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private int monsterID;

    [SerializeField]
    private List <spawnableMonster> monstersToSpawn;
    public List<GameObject> monsters;

    [SerializeField]
    private GameObject Skeleton;

    // Start is called before the first frame update
    void Start()
    {
        spawnableMonster monster = new spawnableMonster();
        monster.spawnChance = 0.5f;
        monster.Prefab = Skeleton;
        monstersToSpawn.Add(monster);
    }

    public void spawnRandomMonster(Vector3 pos)
    {
        
        for (int i = 0; i < monstersToSpawn.Count; i++)
        {
            monsters.Add(monstersToSpawn[i].Prefab);

        }
        Instantiate(monsters[Rand.Range(0, monsters.Count)], pos, Quaternion.identity);

    }
}

public class spawnableMonster
{
    public float spawnChance;
    public GameObject Prefab;
} 
