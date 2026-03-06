using UnityEngine;
using System.Collections.Generic;
public class EnemyEffectsHandler : MonoBehaviour
{
    // Enemy Code Connection
    private EnemyMovement enemyCode;
    
    //Effect Bools
    public bool isPoisoned = false;

    // Timers
    private float poisonTimer = 0f;
    private float poisonHitTimer = 0f;

    private void Awake()
    {
        enemyCode = GetComponent<EnemyMovement>();
    }

    public void ApplyEffectsBasic(List<string> effectlist)
    {
        foreach (var eff in effectlist)
        {
            if (eff == "poison" && Random.Range(0f, 1f) < GlobalPlayerVars.poisonBasicHitPoisonChance)
            {
                poisonTimer = GlobalPlayerVars.poisonPlayerPoisonLength;
                poisonHitTimer = 0f;
                isPoisoned = true;
            }
        }
    }

    public void EffectCheck()
    {
        if (isPoisoned == true)
        {
            Poison();
        }
    }

    public void Poison()
    {
        poisonHitTimer += Time.deltaTime;
        poisonTimer -= Time.deltaTime;

        if (poisonTimer <= 0f)
        {
            isPoisoned = false;
        }
        if (poisonHitTimer >= GlobalPlayerVars.poisonPlayerHitTimer)
        {
            enemyCode.hitSprChanger = .16f;
            GlobalPlayerVars.EnemyHealth -= GlobalPlayerVars.poisonPlayerPoisonDamage;
            enemyCode.CanAtk();
            poisonHitTimer = 0f;
        }
    }
    
}
