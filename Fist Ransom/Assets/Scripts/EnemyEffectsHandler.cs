using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyEffectsHandler : MonoBehaviour
{
    // Enemy Code Connection
    private EnemyMovement enemyCode;
    public AudioSource aS;
    private SpriteRenderer sr;
    private Color originalColor;
    private Color targetColor;
    
    //Effect Bools
    public bool isPoisoned = false;

    // Timers
    private float poisonTimer = 0f;
    private float poisonHitTimer = 0f;

    [Header("Effect Sounds")]
    public AudioClip soundPoisonStart;
    public AudioClip soundPoisonHit;

    [Header("Effect Visuals")]
    public GameObject posionVisual;


    private void Awake()
    {
        enemyCode = GetComponent<EnemyMovement>();
        aS = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }
    public void ApplyEffectsBasic(List<string> effectlist)
    {
        foreach (var eff in effectlist)
        {
            if (eff == "poison" && Random.Range(0f, 1f) < GlobalPlayerVars.poisonBasicHitPoisonChance && !isPoisoned)
            {
                aS.PlayOneShot(soundPoisonStart);
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
            if (poisonHitTimer >= GlobalPlayerVars.poisonPlayerHitTimer)
            {
                Instantiate(posionVisual);
                aS.PlayOneShot(soundPoisonHit);
                enemyCode.hitSprChanger = .16f;

                targetColor = Color.green;
                StartCoroutine(EffectFlicker());

                GlobalPlayerVars.EnemyHealth -= GlobalPlayerVars.poisonPlayerPoisonDamage;
                enemyCode.CanAtk();
                poisonHitTimer = 0f;
            }
        }
    }
    
    IEnumerator EffectFlicker()
    {
        sr.color = targetColor;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }

}
