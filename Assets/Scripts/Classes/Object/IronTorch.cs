using UnityEngine;

public class IronTorch : WorldObject
{
    [SerializeField]
    private Sprite torchSprite;
    protected override void setDefaults()
    {
        OBJECT_NAME = "Iron Torch";
        HEALTH = 30;
        ICON = torchSprite;
    }
}