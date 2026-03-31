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
    public bool isBurning = false;

    // Timers
    private float poisonTimer = 0f;
    private float poisonHitTimer = 0f;
    private float burningTimer = 0f;
    private float burningHitTimer = 0f;

    [Header("Effect Sounds")]
    public AudioClip soundPoisonStart;
    public AudioClip soundPoisonHit;
    public AudioClip soundBurningStart;
    public AudioClip soundBurningHit;

    [Header("Effect Visuals")]
    public GameObject posionVisual;
    public GameObject burningVisual;


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
            if (eff == "burn" && Random.Range(0f, 1f) < GlobalPlayerVars.burnBasicHitBurnChance && !isBurning)
            {
                aS.PlayOneShot(soundBurningStart);
                burningTimer = GlobalPlayerVars.burnPlayerBurnLength;
                burningHitTimer = 0f;
                isBurning = true;
            }
        }
    }

    public void EffectCheck()
    {
        if (isPoisoned)
            Poison();
        if (isBurning)
            Burn();
    }

    private void Poison()
    {
        poisonHitTimer += Time.deltaTime;
        poisonTimer -= Time.deltaTime;

        if (poisonTimer <= 0f)
        {
            isPoisoned = false;
        }
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

    private void Burn()
    {
        burningHitTimer += Time.deltaTime;
        burningTimer -= Time.deltaTime;

        if (burningTimer <= 0f)
        {
            isBurning = false;
        }
        if (burningHitTimer >= 0.3)
        {
            Instantiate(burningVisual);
            aS.PlayOneShot(soundBurningHit);
            targetColor = Color.red;
            StartCoroutine(EffectFlicker());
            GlobalPlayerVars.EnemyHealth -= GlobalPlayerVars.burnPlayerBurnDamage;
            burningHitTimer = 0f;
        }
    }
    
    IEnumerator EffectFlicker()
    {
        sr.color = targetColor;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }

}
