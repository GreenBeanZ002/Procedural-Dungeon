using System.Collections;
using UnityEngine;

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}


[RequireComponent(typeof(SpriteRenderer))]
public abstract class Item : MonoBehaviour
{
    [SerializeField]
    protected string ITEM_NAME;
    public string ItemName => ITEM_NAME;

    [SerializeField]
    protected int MAX_STACK;
    public int MaxStack => MAX_STACK;


    [SerializeField]
    protected string DESCRIPTION;
    public string Description => DESCRIPTION;

    [SerializeField]
    protected int ID;
    public int Id => ID;

    [SerializeField]
    protected Rarity RARITY;
    public Rarity Rarity => RARITY;

    [SerializeField]
    protected Sprite ICON;
    public Sprite Icon => ICON;

    [SerializeField]
    protected int VALUE;
    public int Value => VALUE;


    [SerializeField]
    protected float moveSpeed = 5f;
    [SerializeField]
    protected float pickupRange = 5f;

    private SpriteRenderer spriteRenderer;
    private static Transform playerTransform;
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        setDefaults();
        setupIcon();
        if(playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogWarning("Player not found in the scene. Make sure the player has the 'player' tag.");
            }
        }
    }

    protected virtual void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= pickupRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
    }

    protected void setupIcon()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = ICON;
        }
    }
    public abstract void Use();

    protected abstract void setDefaults();

    public virtual void Pickup(Inventory inventory)
    {
        if(inventory.AddItem(this))
        {
            gameObject.SetActive(false);
        }
    }


}
