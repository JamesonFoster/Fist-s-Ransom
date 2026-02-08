using UnityEngine;

[CreateAssetMenu(fileName = "AtkScriptable", menuName = "Scriptable Objects/AtkScriptable")]
public class AtkScriptable : ScriptableObject
{
    [Header("Pre-Attack Timeings")]
    public float atkWarning = 1f;
    [Header("Attack Damage")]
    public string atkType = "";
    public float atkDamage = 3f;
}
