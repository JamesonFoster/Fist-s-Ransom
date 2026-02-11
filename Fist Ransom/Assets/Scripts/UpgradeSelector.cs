using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeSelector : MonoBehaviour
{
    [Header("Database & Manager")]
    public UpgradeDatabase database;
    public UpgradeManager upgradeManager;

    [Header("UI Buttons")]
    public List<UpgradeButton> buttons = new List<UpgradeButton>();

    [Header("Optional Description Text")]
    public TextMeshProUGUI descriptionText;

    private Dictionary<Rarity, int> rarityWeights = new Dictionary<Rarity, int>
    {
        { Rarity.Common, 60 },
        { Rarity.Rare, 25 },
        { Rarity.Legendary, 10 }
    };

    private void Start()
    {
        if (database == null || upgradeManager == null || buttons.Count == 0)
        {
            Debug.LogError("UpgradeSelector missing references or buttons!");
            return;
        }

        // Auto-assign description text to buttons
        foreach (var btn in buttons)
        {
            if (descriptionText != null)
                btn.descriptionText = descriptionText;

            if (upgradeManager != null)
                btn.upgradeManager = upgradeManager;
        }

        ShowUpgradeChoices();
    }

    private void ShowUpgradeChoices()
    {
        foreach (var btn in buttons)
        {
            Upgrade upgrade = GetRandomUpgrade();
            btn.SetUpgrade(upgrade);
        }
    }

    private Upgrade GetRandomUpgrade()
    {
        List<Upgrade> validUpgrades = new List<Upgrade>();

        foreach (var u in database.allUpgrades)
        {
            if (!upgradeManager.HasUpgrade(u))
                validUpgrades.Add(u);
        }

        if (validUpgrades.Count == 0)
            return null;

        // Weighted random by rarity
        int totalWeight = 0;
        foreach (var u in validUpgrades)
            totalWeight += rarityWeights[u.rarity];

        int roll = Random.Range(0, totalWeight);

        foreach (var u in validUpgrades)
        {
            roll -= rarityWeights[u.rarity];
            if (roll <= 0)
                return u;
        }

        return validUpgrades[0]; // fallback
    }
}
