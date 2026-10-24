using UnityEngine;

public class Crate: WorldObject
{
    [SerializeField]
    private Sprite crateSprite;
    protected override void setDefaults()
    {
        OBJECT_NAME = "Crate";
        HEALTH = 3;
        ICON = crateSprite;
    }
}
