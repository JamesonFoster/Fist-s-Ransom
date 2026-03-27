using UnityEngine;

[CreateAssetMenu(fileName = "BaseEnemyScript", menuName = "Scriptable Objects/BaseEnemyScript")]
public class BaseEnemyScript : ScriptableObject
{
    public string name = "noName";
    public float baseGoldWorth = 100f;
    [Header("Dodging Chances")]
    public float atkRedyPercent = 0.5f;
    public float atkRageRedyPercent = 0.5f;

    [Header("Dodging Stats")]
    public float dodgeDistance = 5f;
    public float dodgeTime = 0.4f;
    public float dodgeStun = 0.1f;

    [Header("Attack Settings")]
    public float atkAgro = 0.02f;
    public float atkSpeedMultiplier = 1f;

    [Header("Health Stats")]
    public float maxHealth = 25f;
    public float postAtkStunTime = 0.6f;
    public float stunnedTime = 2f;
    public float headDamageMultiplier = 1f;
    public float bodyDamageMultiplier = 1f;

    [Header("Sounds")]
    public AudioClip soundHit1;
    public AudioClip soundHit2;
    public AudioClip soundDeath;
    public AudioClip soundDodge;
    public AudioClip soundStunned;
    public AudioClip soundCele1;
    public AudioClip soundCele2;

    [Header("Sprites")]
    public float idlespeed = 0.5f;
    public Sprite sprStandingStill;
    public Sprite sprStandingStill2;
    public Sprite sprStunned1;
    public Sprite sprStunned2;
    public Sprite sprDead;
    public Sprite sprDodge;
    public Sprite sprDodgeL;
    public Sprite sprHeadHitL;
    public Sprite sprHeadHitR;
    public Sprite sprBodyHitL;
    public Sprite sprBodyHitR;
    public Sprite sprPlayerDeath1;
    public Sprite sprPlayerDeath2;

    [Header("Personality Values")]
    public bool holdOn = false;
    [System.Serializable]
    public class AtkNHittableSettings
    {
    public bool postHitAtker = false;
    public bool postDodgeAtker = false;
    public bool isSlippery = false;
    public bool unharmableVoidStun = false;
    }
    [System.Serializable]
    public class ModeShiftSettings
    {
        public float modeShiftSpeed = 0f;
        public BaseEnemyScript modeShift;
    }
    [System.Serializable]
    public class ItemTestingSettings
    {
        public Upgrade ifPlayerHas;
        public bool isModeShift = false;
        public BaseEnemyScript modeToShift2;
    }
    public AtkNHittableSettings atkNHitSettings;
    public ModeShiftSettings modeShiftSettings;
    public ItemTestingSettings itemTestingSettings;
}
