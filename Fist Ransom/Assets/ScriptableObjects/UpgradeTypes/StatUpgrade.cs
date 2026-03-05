using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "StatUpgrade", menuName = "Upgrades/StatUpgrade")]
public class StatUpgrade : Upgrade
{
    [Header("Static Upgrades")]
    public float headDamageIncrease = 0f;
    public float bodyDamageIncrease = 0f;
    public float headRageIncrease = 0f;
    public float bodyRageIncrease = 0f;
    public float maxHealthIncrease = 0f;
    public float dodgeTimeIncrease = 0f;
    public float goldMultiplyier = 0f;


    [Header("Multipitive  Upgrades")]
    public float attackCoolDec = 0f;
    public float dodgeStunDec = 0f;



    public override void Apply()
    {
        // Modify your GlobalPlayerVars
        GlobalPlayerVars.headAtkDama += headDamageIncrease;
        GlobalPlayerVars.bodyAtkDama += bodyDamageIncrease;
        GlobalPlayerVars.rageBodyAtk += bodyRageIncrease;
        GlobalPlayerVars.rageHeadAtk += headDamageIncrease;
        GlobalPlayerVars.PlayerMaxHealth += maxHealthIncrease;
        GlobalPlayerVars.PlayerHealth += maxHealthIncrease;
        GlobalPlayerVars.dodgeTime += dodgeStunDec;
        GlobalPlayerVars.coinMultiplay += goldMultiplyier;

        //Speed Stuff
        float actuatkCool = 1 - attackCoolDec;
        float actudodCool = 1 - dodgeStunDec;
        GlobalPlayerVars.atkCooldown *= actuatkCool;
        GlobalPlayerVars.dodgeStun *= actudodCool;
    }
}
