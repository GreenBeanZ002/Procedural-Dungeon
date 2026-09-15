using UnityEngine;

using Rand = UnityEngine.Random;

public class smallExpOrb : Item
{
    [SerializeField] private Sprite expOrbSprite;
    protected override void setDefaults()
    {
        ITEM_NAME = "Experience Orb";
        DESCRIPTION = "A glowing orb that grants experience.";
        VALUE = Rand.Range(1, 11);
        ICON = expOrbSprite;
    }
    public override void Use()
    {
        // Experience orbs cannot be "used" directly, so this stays empty
    }
    public override void Pickup(Inventory inventory)
    {
        // Assuming you have an experience manager to handle experience points
        LevelUtils.addExp(VALUE);
        Destroy(gameObject);
        Debug.Log("Picked up experience orb");
    }
}
