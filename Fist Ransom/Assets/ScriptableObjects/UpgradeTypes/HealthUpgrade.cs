using UnityEngine;

[CreateAssetMenu(fileName = "New Health Upgrade", menuName = "Upgrades/Health Upgrade")]
public class HealthUpgrade : Upgrade
{
    public float maxHealthIncrease = 2f;

    public override void Apply()
    {
        // Modify your GlobalPlayerVars
        GlobalPlayerVars.PlayerMaxHealth += maxHealthIncrease;
        GlobalPlayerVars.PlayerHealth += maxHealthIncrease;

        Debug.Log($"{upgradeName} applied!");
    }
}
