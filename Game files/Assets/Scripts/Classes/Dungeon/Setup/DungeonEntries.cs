// DungeonEntries.cs
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class ObjectEntry
{
    public WorldObject objectPrefab;
    [Range(0, 100)] public float spawnChance;
}

[System.Serializable]
public class MonsterEntry
{
    public GameObject monsterPrefab;
    [Range(0, 100)] public float spawnChance;
}

[System.Serializable]
public class FloorEntry
{
    public TileBase floorTile;
    [Range(0, 100)] public float spawnChance;
}

[System.Serializable]
public class WallEntry
{
    public TileBase wallTile;
    [Range(0, 100)] public float spawnChance;
}