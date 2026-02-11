using UnityEngine;
using System.Collections.Generic; 
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private HashSet<Upgrade> ownedUpgrades = new HashSet<Upgrade>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // keeps this object across scenes
    }

    public void AddUpgrade(Upgrade upgrade)
    {
        if (ownedUpgrades.Contains(upgrade)) return;
        ownedUpgrades.Add(upgrade);
        upgrade.Apply();
    }

    public bool HasUpgrade(Upgrade upgrade)
    {
        return ownedUpgrades.Contains(upgrade);
    }
}
