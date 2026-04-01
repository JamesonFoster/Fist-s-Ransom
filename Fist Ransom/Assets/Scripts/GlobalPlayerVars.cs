using UnityEngine;
using System.Collections.Generic;
public class GlobalPlayerVars : MonoBehaviour
{
    //PlayerMapInfo
    public static int playerLocationID = 0;
    public static int playerAct = 1;

    //PlayerValues
    public static int HealCount = 3;
    public static int RageCount = 1;
    public static float PlayerHealth = 100;
    public static float PlayerMaxHealth = 100;
    public static float PlayerRegenPer = 0f;
    public static float PlayerRageSpeed = 1f;

    //Rage Bar Stats
    public static int PlayerRage = 0;
    public static int PlayerRagePerAtk = 5;
    public static int PlayerRageMax = 100;
    public static int AleRageAmount = 25;

    //Attacking Stats
    public static float atkCooldown = 0.4f;
    public static float headAtkDama = 3f;
    public static float bodyAtkDama = 3;
    public static float rageHeadAtk = 10;
    public static float rageBodyAtk = 10;
    public static float hitStunnedLength = 0.5f;
    public static List<string> effectsList = new List<string>();

    //Dodging Stats
    public static float dodgeDistance = 0.5f;
    public static float dodgeTime = 0.4f;
    public static float dodgeStun = 0.1f;
    public static float dodgingRageNullifier = 0f;

    //Coins
    public static int gold = 0;
    public static float coinMultiplay = 1f;

    //EnemyValues
    public static float EnemyHealth;
    public static float EnemyMaxHealth;
    public static string EnemyName;
    public static float goldvalue;


    //Player Bool Upgrade Values
    public static bool scyllaAxe = false;
    public static bool scyllaSoul = false;
    public static bool scyllaCoat = false;


    // Player Effect Values
        //Poison Values
    public static bool poisonRageHit = false;
    public static float poisonBasicHitPoisonChance = 0f;
    public static float poisonPlayerPoisonLength = 10f;
    public static float poisonPlayerPoisonDamage = 5f;
    public static float poisonPlayerHitTimer = 2.5f;
        //Burning Values
    public static bool burnRageHit = false;
    public static float burnBasicHitBurnChance = 0f;
    public static float burnPlayerBurnLength = 3f;
    public static float burnPlayerBurnDamage = 0.5f;
}
