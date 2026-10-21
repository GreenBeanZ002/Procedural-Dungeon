using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class DungeonArea : MonoBehaviour
{
    [SerializeField]
    public List<ObjectEntry> objectsToSpawn;

    [SerializeField]
    public List<MonsterEntry> monstersToSpawn;

    [SerializeField]
    public List<FloorEntry> floorTilesToSpawn;

    [SerializeField]
    public List<WallEntry> wallTilesToSpawn;
    [SerializeField]
    protected string AREA_NAME;
    public string AreaName => AREA_NAME;

    [SerializeField]
    protected string LEVEL_COUNT;
    public string LevelCount => LEVEL_COUNT;

    [SerializeField]
    protected bool AREa_DEFAULT;

    public bool AreaDefault => AREa_DEFAULT;

    [SerializeField]
    protected int BOSS_LEVEL;

    public int BossLevel => BOSS_LEVEL;

    public bool isBossLevel(int level)
    {
        if (level == BOSS_LEVEL)
        {
            return true;
        }
        return false;
    }
    protected abstract void SetDefaults();

    protected virtual void Awake()
    {
        SetDefaults();
    }
}
