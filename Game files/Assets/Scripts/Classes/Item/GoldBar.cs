using UnityEngine;

public class GoldBar: Item
{
    [SerializeField] private Sprite goldBarSprite;

    protected override void setDefaults()
    {
        ITEM_NAME = "Gold Bar";
        DESCRIPTION = "A shiny gold bar.";
        VALUE = Random.Range(5, 10);
        ICON = goldBarSprite;
    }
    public override void Use()
    {
        //gold bars cannot actually be "used" so this stays empty
    }

    public override void Pickup(Inventory inventory)
    {
        coinsManager.Instance.addSpecificAmtOfCoins(VALUE);
        Destroy(gameObject);
        Debug.Log("Picked up gold bar");
    }
}