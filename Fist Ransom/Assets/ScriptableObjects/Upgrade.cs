using UnityEngine;

public enum Rarity
{
    Common,
    Rare,
    Legendary
}

// Abstract base class for all upgrades
public abstract class Upgrade : ScriptableObject
{
    public string upgradeName;
    public Rarity rarity;
    public Sprite icon;

    [TextArea]
    public string description;

    // Each upgrade must implement this to apply its effect
    public abstract void Apply();
}
