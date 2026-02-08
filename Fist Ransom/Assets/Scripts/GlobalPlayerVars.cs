using UnityEditor.PackageManager;
using UnityEngine;

public class GlobalPlayerVars : MonoBehaviour
{
    //PlayerValues
    public static int HealCount = 3;
    public static int RageCount = 1;
    public static float PlayerHealth = 50;
    public static float PlayerMaxHealth = 100;
    public static int PlayerRage = 0;

    //EnemyValues
    public static float EnemyHealth;
    public static float EnemyMaxHealth;
    public static string EnemyName;
}
