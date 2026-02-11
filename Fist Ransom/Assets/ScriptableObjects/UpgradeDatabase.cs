using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Upgrades/Upgrade Database")]
public class UpgradeDatabase : ScriptableObject
{
    public List<Upgrade> allUpgrades;  // <-- Add your new upgrade here
}
