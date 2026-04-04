using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolUpgrade", menuName = "Upgrades/BoolUpgrade")]
public class BoolUpgrade : Upgrade
{
    [Header("Scylla Upgrades")]
    public bool scyllaCoat = false;
    public bool scyllaAxe = false;
    public bool scyllaSoul = false;

    public override void Apply()
    {
        GlobalPlayerVars.scyllaAxe = scyllaAxe;
        GlobalPlayerVars.scyllaCoat = scyllaCoat;
        GlobalPlayerVars.scyllaSoul = scyllaSoul;
        if (scyllaAxe)
        {
            GlobalPlayerVars.heatDecreasingPer = 0.02f;
            GlobalPlayerVars.rageBodyAtk += 4f;
            GlobalPlayerVars.rageHeadAtk += 4f;
            GlobalPlayerVars.dodgingRageNullifier += 0.05f;
        }
        if (scyllaSoul)
        {
            GlobalPlayerVars.heatDecreasingPer = 0.02f;
            GlobalPlayerVars.PlayerMaxHealth += 30f;
            GlobalPlayerVars.PlayerHealth += 30f;
            GlobalPlayerVars.PlayerRegenPer += 1f;
        }
        if (scyllaCoat)
        {
            GlobalPlayerVars.heatDecreasingPer = 0.02f;
            GlobalPlayerVars.dodgeStun += 0.1f;
        }
    }

     public override void Remove()
    {
        GlobalPlayerVars.scyllaAxe = !scyllaAxe;
        GlobalPlayerVars.scyllaCoat = !scyllaCoat;
        GlobalPlayerVars.scyllaSoul = !scyllaSoul;
        if (scyllaAxe)
        {
            GlobalPlayerVars.heatDecreasingPer = 0.04f;
            GlobalPlayerVars.rageBodyAtk -= 4f;
            GlobalPlayerVars.rageHeadAtk -= 4f;
            GlobalPlayerVars.dodgingRageNullifier -= 0.05f;
        }
        if (scyllaSoul)
        {
            GlobalPlayerVars.heatDecreasingPer = 0.04f;
            GlobalPlayerVars.PlayerMaxHealth -= 30f;
            GlobalPlayerVars.PlayerHealth -= 30f;
            GlobalPlayerVars.PlayerRegenPer -= 1f;
        }
        if (scyllaCoat)
        {
            GlobalPlayerVars.heatDecreasingPer = 0.04f;
            GlobalPlayerVars.dodgeStun -= 0.1f;
        }
    }
}
