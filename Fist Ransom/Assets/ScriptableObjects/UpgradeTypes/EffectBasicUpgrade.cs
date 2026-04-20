using JetBrains.Annotations;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EffectBasicUpgrade", menuName = "Upgrades/EffectBasicUpgrade")]
public class EffectBasicUpgrade : Upgrade
{
    [Header("Poison Upgrades")]
    public bool ragePoison = false;
    public float basicHitPoisonChanceIncre = 0f;
    public float poisonLengthIncre = 0f;
    public float poisonHitDamageIncre = 0f;
    public float poisonHitTimerDecre = 0f;

    [Header("Burn Upgrades")]
    public bool rageBurn = false;
    public float basicHitBurnChanceIncre = 0f;
    public float burnLengthIncre = 0f;
    public float burnHitDamageIncre = 0f;



    public override void Apply()
    {
        // Poison Applys
        if ((ragePoison == true || basicHitPoisonChanceIncre != 0f) && !GlobalPlayerVars.effectsList.Contains("poison"))
            GlobalPlayerVars.effectsList.Add("poison");
        if (ragePoison == true)
            GlobalPlayerVars.poisonRageHit = true;
        GlobalPlayerVars.poisonBasicHitPoisonChance += basicHitPoisonChanceIncre;
        GlobalPlayerVars.poisonPlayerHitTimer -= poisonHitTimerDecre;
        GlobalPlayerVars.poisonPlayerPoisonDamage += poisonHitDamageIncre;
        GlobalPlayerVars.poisonPlayerPoisonLength += poisonLengthIncre;

        // Burn Applys
        if ((rageBurn == true || basicHitBurnChanceIncre != 0f) && !GlobalPlayerVars.effectsList.Contains("burn"))
            GlobalPlayerVars.effectsList.Add("burn");
        if (rageBurn == true)
            GlobalPlayerVars.burnRageHit = true;
        GlobalPlayerVars.burnBasicHitBurnChance += basicHitBurnChanceIncre;
        GlobalPlayerVars.burnPlayerBurnDamage += burnHitDamageIncre;
        GlobalPlayerVars.burnPlayerBurnLength += burnLengthIncre;
    }

    public override void Remove()
    {
        // Poison Applys
        if ((ragePoison == true || basicHitPoisonChanceIncre != 0f) && GlobalPlayerVars.effectsList.Contains("poison"))
            GlobalPlayerVars.effectsList.Remove("poison");
        if (ragePoison == true)
            GlobalPlayerVars.poisonRageHit = false;
        GlobalPlayerVars.poisonBasicHitPoisonChance -= basicHitPoisonChanceIncre;
        GlobalPlayerVars.poisonPlayerHitTimer += poisonHitTimerDecre;
        GlobalPlayerVars.poisonPlayerPoisonDamage -= poisonHitDamageIncre;
        GlobalPlayerVars.poisonPlayerPoisonLength -= poisonLengthIncre;

        // Burn Applys
        if ((rageBurn == true || basicHitBurnChanceIncre != 0f) && GlobalPlayerVars.effectsList.Contains("burn"))
            GlobalPlayerVars.effectsList.Remove("burn");
        if (rageBurn == true)
            GlobalPlayerVars.burnRageHit = false;
        GlobalPlayerVars.burnBasicHitBurnChance -= basicHitBurnChanceIncre;
        GlobalPlayerVars.burnPlayerBurnDamage -= burnHitDamageIncre;
        GlobalPlayerVars.burnPlayerBurnLength -= burnLengthIncre;
    }
}
