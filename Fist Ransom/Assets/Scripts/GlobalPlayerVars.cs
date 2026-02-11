using UnityEditor.PackageManager;
using UnityEngine;

public class GlobalPlayerVars : MonoBehaviour
{
    //PlayerValues
    public static int HealCount = 3;
    public static int RageCount = 1;
    public static float PlayerHealth = 100;
    public static float PlayerMaxHealth = 100;
    public static int PlayerRage = 0;


    //Attacking Stats
    public static float atkCooldown = 0.4f;
    public static float headAtkDama = 3f;
    public static float bodyAtkDama = 3;
    public static float rageHeadAtk = 10;
    public static float rageBodyAtk = 10;

    //Dodging Stats
    public static float dodgeDistance = 0.5f;
    public static float dodgeTime = 0.4f;
    public static float dodgeStun = 0.1f;



    //EnemyValues
    public static float EnemyHealth;
    public static float EnemyMaxHealth;
    public static string EnemyName;
}
