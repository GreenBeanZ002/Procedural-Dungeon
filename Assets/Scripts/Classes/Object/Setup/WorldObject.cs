using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;

public abstract class WorldObject : MonoBehaviour
{
    [SerializeField]
    private AudioSource breakSound;
    [SerializeField]
    private float shakeAmount = 0.1f;

    [SerializeField]
    protected string OBJECT_NAME;
    public string ObjectName => OBJECT_NAME;

    [SerializeField]
    protected Sprite ICON;
    public Sprite Icon => ICON;

    [SerializeField]
    protected int HEALTH;

    public int Health => HEALTH;

    private SpriteRenderer spriteRenderer;
    private DropTable dropTable;

    private bool vibrating;
    private float currentDuration;
    private int currentHealth;
    private Vector3 initialPos;

    protected virtual void Awake()
    {
        setDefaults();
        spriteRenderer = GetComponent<SpriteRenderer>();
        setupObject();

        currentHealth = HEALTH;
        vibrating = false;
        dropTable = GetComponent<DropTable>();
    }

    protected void setupObject()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = ICON;
        }
    }

    public virtual void Damage(float shakeDuration, int DamageTaken)
    {
        vibrating = true;
        currentHealth -= DamageTaken;
        breakSound.Play();
        initialPos = transform.position;   // capture current position FIRST
        currentDuration = shakeDuration;
        DamageEffect();                    // now uses the correct initialPos

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    protected virtual void HandleDeath()
    {
        dropTable.DropAll(transform.position);

        if (breakSound != null && breakSound.clip != null)
        {
            AudioSource.PlayClipAtPoint(breakSound.clip, transform.position, breakSound.volume);
        }

        Destroy(gameObject);
    }

    private void DamageEffect()
    {
        if (currentDuration > 0)
        {
            Vector3 offset = Rand.insideUnitSphere * shakeAmount;
            offset.z = 0;
            transform.position = initialPos + offset;
            currentDuration -= Time.deltaTime;
        }
        else
        {
            vibrating = false;
            transform.position = initialPos;
        }
    }

    protected abstract void setDefaults();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("playerAttack"))
        {
            Player player = FindObjectOfType<Player>();
            Damage(0.1f, player.GetDamage());
            Destroy(collision.collider.gameObject);
        }
    }

    protected virtual void Update()
    {
        if (vibrating)
        {
            DamageEffect();
        }
    }
}

