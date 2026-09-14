using System.Collections.Generic;
using UnityEngine;

public static class RarityColours
{
    public static readonly Dictionary<Rarity, Color> Colours = new Dictionary<Rarity, Color>
    {
        { Rarity.Common, Color.white },
        { Rarity.Uncommon, Color.green },
        { Rarity.Rare, Color.blue },
        { Rarity.Epic, new Color(0.6f, 0f, 0.6f) }, // Purple
        { Rarity.Legendary, new Color(1f, 0.5f, 0f) } // Orange
    };
}