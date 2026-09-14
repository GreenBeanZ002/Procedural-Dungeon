using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarLogic : MonoBehaviour
{
    public float health, maxHealth, width, height;

    [SerializeField] private RectTransform healthBar;
    // Start is called before the first frame update
    private void SetMaxHealth()
    {
        maxHealth = HealthScript.GetMaxHealth();
    }

    private void SetHealth()
    {
        health = HealthScript.GetHealth();
    }

    private void setWidth()
    {
        SetMaxHealth();
        SetHealth();
        float newWidth = (health / maxHealth) * width;
        healthBar.sizeDelta = new Vector2(newWidth, height);
    } 
    // Update is called once per frame
    void Update()
    {
        setWidth();
    }
}
