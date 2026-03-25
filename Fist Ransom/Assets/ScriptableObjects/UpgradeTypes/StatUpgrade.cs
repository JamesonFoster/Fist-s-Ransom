using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "StatUpgrade", menuName = "Upgrades/StatUpgrade")]
public class StatUpgrade : Upgrade
{
    [Header("Basic Damage Upgrades")]
    public float headDamageIncrease = 0f;
    public float bodyDamageIncrease = 0f;
    public float headRageIncrease = 0f;
    public float bodyRageIncrease = 0f;
    [Header("Health Upgrades")]
    public float maxHealthIncrease = 0f;
    public float regenPer5SecondsInc = 0f;
    [Header("Rage Upgrades")]
    public int rageMaxInc = 0;
    public int ragePerHitInc = 0;
    public int aleRageInc = 0;

    [Header("Misc Upgrades")]
    public float dodgeTimeIncrease = 0f;
    public float goldMultiplyier = 0f;
    public float rageDodgeNullIncre = 0f;


    [Header("Multipitive  Upgrades")]
    public float attackCoolDec = 0f;
    public float dodgeStunDec = 0f;
    public float rageAtkSpeedInc = 0f;



    public override void Apply()
    {
        // Modify your GlobalPlayerVars
        GlobalPlayerVars.headAtkDama += headDamageIncrease;
        GlobalPlayerVars.bodyAtkDama += bodyDamageIncrease;
        GlobalPlayerVars.rageBodyAtk += bodyRageIncrease;
        GlobalPlayerVars.rageHeadAtk += headRageIncrease;
        GlobalPlayerVars.PlayerMaxHealth += maxHealthIncrease;
        GlobalPlayerVars.PlayerHealth += maxHealthIncrease;
        GlobalPlayerVars.PlayerRegenPer += regenPer5SecondsInc;
        GlobalPlayerVars.dodgeTime += dodgeStunDec;
        GlobalPlayerVars.coinMultiplay += goldMultiplyier;
        GlobalPlayerVars.dodgingRageNullifier += rageDodgeNullIncre;
        GlobalPlayerVars.PlayerRagePerAtk += ragePerHitInc;
        GlobalPlayerVars.PlayerRageMax += rageMaxInc;
        GlobalPlayerVars.AleRageAmount += aleRageInc;

        //Speed Stuff
        float actuatkCool = 1 - attackCoolDec;
        float actudodCool1 = 1 - dodgeStunDec;
        float actuatkCool2 = 1 - rageAtkSpeedInc;
        GlobalPlayerVars.atkCooldown *= actuatkCool;
        GlobalPlayerVars.dodgeStun *= actudodCool1;
        GlobalPlayerVars.PlayerRageSpeed *= actuatkCool2;
    }
}
