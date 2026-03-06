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
    }
}
