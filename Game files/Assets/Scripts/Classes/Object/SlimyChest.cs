using UnityEngine;

public class  SlimyChest : WorldObject
{
    [SerializeField]
    private Sprite chestSprite;

    [SerializeField]
    private GameObject SlimeEnemy;
        protected override void setDefaults()
    {
        OBJECT_NAME = "Slimy Chest";
        HEALTH = 7;
        ICON = chestSprite;
    }

    protected override void DeathEvent()
    {
        SummonMultipleEnemies(transform.position, 3, SlimeEnemy);
    }
    private void SummonMultipleEnemies(Vector3 pos, int quantity, GameObject prefab)
    {
        for (int i = 0; i < quantity; i++)
        {
            Instantiate(prefab, pos, Quaternion.identity);
            Debug.Log("Summoned enemy from chest");
        }
        return;
    }
}
