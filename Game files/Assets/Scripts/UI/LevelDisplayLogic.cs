using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class levelDisplayLogic : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;

    [SerializeField] private Image levelBadgeImage;

    [SerializeField] private Sprite levelBadge1, levelBadge2, levelBadge3, levelBadge4;
    void Update()
    {
        int exp = LevelUtils.getExp();
        int level = LevelUtils.getLevel(exp);
        levelText.text = level.ToString();
        if (level <= 25)
        {
            levelBadgeImage.sprite = levelBadge1;
        }
        else if (level <= 50)
        {
            levelBadgeImage.sprite = levelBadge2;
        }
        else if (level <= 75)
        {
            levelBadgeImage.sprite = levelBadge3;
        }
        else
        {
            levelBadgeImage.sprite = levelBadge4;
        }
    }
}
