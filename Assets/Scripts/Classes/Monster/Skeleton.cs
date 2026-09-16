using UnityEngine;

public class Skeleton : Monster
{
    [SerializeField] private Sprite monsterSprite;

    protected override void SetDefaults()
    {
        MONSTER_NAME = "Skeleton";
        MONSTER_HEALTH = 10;
        MONSTER_SPEED = 5f;
        MONSTER_DAMAGE = 10;
        MONSTER_SPRITE = monsterSprite;
    }
}

