using UnityEngine;

public class Coin : Item
{
    [SerializeField] private Sprite coinSprite;

    protected override void setDefaults()
    {
        ITEM_NAME = "Coin";
        DESCRIPTION = "A shiny gold coin.";
        VALUE = Random.Range(1, 3);
        ICON = coinSprite;
    }
    public override void Use()
    {
        //coins cannot actually be "used" so this stays empty
    }

    public override void Pickup(Inventory inventory)
    {
        coinsManager.Instance.addSpecificAmtOfCoins(VALUE);
        Destroy(gameObject);
        Debug.Log("Picked up coin");
    }
}
