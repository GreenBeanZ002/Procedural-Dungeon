using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarLogic : MonoBehaviour
{
    public float exp, maxExp, width, height;
    [SerializeField] private RectTransform expBar;
    // Start is called before the first frame update

    public void SetMaxExp()
    {
        int currentLevel = LevelUtils.getLevel(LevelUtils.getExp());
        maxExp = (float)LevelUtils.getExpToLevel(currentLevel + 1) - (float)LevelUtils.getExpToLevel(currentLevel);
    }

    public void SetExp()
    {
        int currentLevel = LevelUtils.getLevel(LevelUtils.getExp());
        exp = LevelUtils.getExp() - LevelUtils.getExpToLevel(currentLevel);
    }

    private void setWidth()
    {
        SetMaxExp();
        SetExp();
        float newWidth = (exp / maxExp) * width;
        expBar.sizeDelta = new Vector2(newWidth, height);
        SetMaxExp();
        SetExp();
    }

    private void Update()
    {
        setWidth();
        if (Input.GetKeyDown(KeyCode.V))
        {

            int currentLevel = LevelUtils.getLevel(LevelUtils.getExp());

            Debug.Log("Exp: " + exp + " / " + maxExp + " (" + LevelUtils.getLevel(LevelUtils.getExp()) + ")");

        }
    }

}
