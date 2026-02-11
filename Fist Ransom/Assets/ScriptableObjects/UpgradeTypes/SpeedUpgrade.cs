using UnityEngine;

[CreateAssetMenu(fileName = "New Speed Upgrade", menuName = "Upgrades/Speed Upgrade")]
public class SpeedUpgrade : Upgrade
{
    public float attackCoolDec = 0.1f;
    public float dodgeStunDec = 0.1f;

    public override void Apply()
    {
        float actuatkCool = 1 - attackCoolDec;
        float actudodCool = 1 - dodgeStunDec;
        // Modify your GlobalPlayerVars
        GlobalPlayerVars.atkCooldown *= actuatkCool;
        GlobalPlayerVars.dodgeStun *= actudodCool;

        Debug.Log($"{upgradeName} applied!");
    }
}
