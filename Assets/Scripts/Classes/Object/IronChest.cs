using UnityEngine;

public class IronChest : WorldObject
{
    [SerializeField]
    private Sprite chestSprite;
    protected override void setDefaults()
    {
        OBJECT_NAME = "Iron Chest";
        HEALTH = 9;
        ICON = chestSprite;
    }
}
