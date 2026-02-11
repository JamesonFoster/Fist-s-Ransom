using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Upgrade", menuName = "Upgrades/Damage Upgrade")]
public class DamageUpgrade : Upgrade
{
    public float headDamageIncrease = 2f;
    public float bodyDamageIncrease = 2f;
    public float headRageIncrease = 0f;
    public float bodyRageIncrease = 0f;

    public override void Apply()
    {
        // Modify your GlobalPlayerVars
        GlobalPlayerVars.headAtkDama += headDamageIncrease;
        GlobalPlayerVars.bodyAtkDama += bodyDamageIncrease;
        GlobalPlayerVars.rageBodyAtk += bodyRageIncrease;
        GlobalPlayerVars.rageHeadAtk += headDamageIncrease;

        Debug.Log($"{upgradeName} applied! Head damage: {GlobalPlayerVars.headAtkDama}, Body damage: {GlobalPlayerVars.bodyAtkDama}");
    }
}
