using UnityEngine;
using System.Collections.Generic;

public enum AtkChaining
{
    Single,
    Repeat,
    Domino
}

[CreateAssetMenu(fileName = "AtkScriptable", menuName = "Scriptable Objects/AtkScriptable")]
public class AtkScriptable : ScriptableObject
{
    [Header("Pre-Attack Timeings")]
    public float atkWarning = 1f;
    public float parryTime = 0.1f;
    public float postAtkDodgeStun;

    [Header("Attack Damage")]
    public string atkType = "";
    public List<string> attackEffects;
    public float atkDamage = 3f;

    [Header("Sprites")]
    public Sprite sprAttackWarning;
    public Sprite sprAttack;

    [Header("Sounds")]
    public AudioClip soundAttack;
    public AudioClip warnAttack;
    public bool playWarning = false;

    [Header("Parry Values")]
    public bool parryableUpL = true;
    public bool parryableUpR = true;
    public bool parryableR = true;
    public bool parryableL = true;

    [Header("Attack Special Values")]
    public bool unstunable = false;
    public bool unparryable = false;
    public bool isntAtker = false;
    public bool noMoveBackWarning = false;

    public AtkChaining atkChaining;

    [HideInInspector]
    public int howManyTime = 0;

    [HideInInspector]
    public AtkScriptable nextAtk;
}
