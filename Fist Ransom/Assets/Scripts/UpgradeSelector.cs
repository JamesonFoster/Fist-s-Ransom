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

    // -------- RARITY WEIGHTS --------

    [System.Serializable]
    public class RarityWeight
    {
        public Rarity rarity;
        public int weight = 0;
    }

    [Header("Rarity Weights")]
    public List<RarityWeight> rarityWeights = new List<RarityWeight>();

    // --------------------------------

    private void Start()
    {
        if (database == null || upgradeManager == null || buttons.Count == 0)
        {
            Debug.LogError("UpgradeSelector missing references or buttons!");
            return;
        }

        // Assign references to buttons
        foreach (var btn in buttons)
        {
            if (descriptionText != null)
                btn.descriptionText = descriptionText;

            btn.upgradeManager = upgradeManager;
        }

        ShowUpgradeChoices();
    }

    private void ShowUpgradeChoices()
    {
        List<Upgrade> alreadyChosen = new List<Upgrade>();

        foreach (var btn in buttons)
        {
            Upgrade upgrade = GetRandomUpgrade(alreadyChosen);
            alreadyChosen.Add(upgrade);
            btn.SetUpgrade(upgrade);
        }
    }

    private Upgrade GetRandomUpgrade(List<Upgrade> excludeList)
    {
        List<Upgrade> validUpgrades = new List<Upgrade>();

        foreach (var u in database.allUpgrades)
        {
            if (!upgradeManager.HasUpgrade(u) && !excludeList.Contains(u))
                validUpgrades.Add(u);
        }

        if (validUpgrades.Count == 0)
            return null;

        int totalWeight = 0;

        foreach (var u in validUpgrades)
        {
            totalWeight += GetWeight(u.rarity);
        }

        if (totalWeight <= 0)
            return validUpgrades[Random.Range(0, validUpgrades.Count)];

        int roll = Random.Range(0, totalWeight);

        foreach (var u in validUpgrades)
        {
            roll -= GetWeight(u.rarity);
            if (roll < 0)
                return u;
        }

        return validUpgrades[0];
    }

    private int GetWeight(Rarity rarity)
    {
        foreach (var rw in rarityWeights)
        {
            if (rw.rarity == rarity)
                return Mathf.Max(0, rw.weight);
        }

        return 0;
    }

    // Automatically ensure every rarity exists in the list
    private void OnValidate()
    {
        foreach (Rarity r in System.Enum.GetValues(typeof(Rarity)))
        {
            if (!rarityWeights.Exists(x => x.rarity == r))
            {
                rarityWeights.Add(new RarityWeight { rarity = r, weight = 0 });
            }
        }
    }
}