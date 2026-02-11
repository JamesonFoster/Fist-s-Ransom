using UnityEngine;

[CreateAssetMenu(fileName = "BaseEnemyScript", menuName = "Scriptable Objects/BaseEnemyScript")]
public class BaseEnemyScript : ScriptableObject
{
    public string name = "noName";
    [Header("Dodging Chances")]
    public float atkRedyPercent = 0.5f;

    [Header("Dodging Stats")]
    public float dodgeDistance = 5f;
    public float dodgeTime = 0.4f;
    public float dodgeStun = 0.1f;

    [Header("Attack Basic Stats")]
    public float atkAgro = 0.02f;
    public float atkWarning = 1f;

    [Header("Attack Damage")]
    public string atkType = "";
    public float atkDamage = 3f;

    [Header("Health Stats")]
    public float maxHealth = 25f;
    public float postAtkStunTime = 0.6f;
    public float stunnedTime = 2f;

    [Header("Sprites")]
    public Sprite sprStandingStill;
    public Sprite sprAtkSwing;
    public Sprite sprAtkWarn;
    public Sprite sprStunned1;
    public Sprite sprStunned2;
    public Sprite sprDead;
    public Sprite sprDodge;
    public Sprite sprHeadHitL;
    public Sprite sprHeadHitR;
}
