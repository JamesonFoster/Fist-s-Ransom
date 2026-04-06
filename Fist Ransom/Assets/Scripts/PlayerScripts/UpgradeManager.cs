using UnityEngine;
using System.Collections.Generic; 
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private HashSet<Upgrade> ownedUpgrades = new HashSet<Upgrade>();

    public void AddUpgrade(Upgrade upgrade)
    {
        ownedUpgrades.Add(upgrade);
        upgrade.Apply();
    }

    public bool HasUpgrade(Upgrade upgrade)
    {
        return ownedUpgrades.Contains(upgrade);
    }

    public void ClearUpgrades()
    {
        foreach (Upgrade upgrade in ownedUpgrades)
        {
            upgrade.Remove(); // undo effect
        }

        ownedUpgrades.Clear();
    }
}
