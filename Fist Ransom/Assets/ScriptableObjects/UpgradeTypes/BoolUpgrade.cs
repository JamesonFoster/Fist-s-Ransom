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
    }
}
