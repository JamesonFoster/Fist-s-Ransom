using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class EnemyMovement : MonoBehaviour
{
    [Header("Enemy Data")]
    public BaseEnemyScript enemyData;
    public int phase = 0;


    [Header("Boss")]
    public BossPhaseController BPC;
    public GameObject winScreen;


    [Header("Connections")]
    public PlayerAtk target;
    public PlayerMovement target2;


    [Header("UI")]
    public GameObject damageTextPrefab;
    public Transform canvasTransform;


    [Header("Attack List")]
    public List<AtkScriptable> listOfAttacks;



    // Components
    private SpriteRenderer sprrend;
    public AudioSource aS;
    private EnemyEffectsHandler enemyEff;


    // Position
    private Vector2 dodgeTarget;
    private Vector2 startPos;
    private Vector2 corePos;


    // Slippery movement
    private float xChanger = 0;
    private float yChanger = 0;


    // Dodge
    private bool isDodging = false;
    private float dodgeTimer = 0f;
    private float stunTimer = 999f;


    // Attack
    private float timerAtk = 0f;
    private bool isAtk = false;

    private Sprite sprATKWARN;
    private Sprite sprATK;
    private float atkDAMA;
    private float parTime;
    private float atkWARN;
    private int countdownAtk;
    private AtkScriptable nextAtk;
    private AtkScriptable atkChoose;
    private bool isparryable;
    private bool atkSoundCheck;
    private bool soundcheck2 = false;


    // Stun
    private bool stunable = false;
    private float stunableTimer = 0f;
    private float stunnedTimer = 0f;
    public bool stunned = false;

    private bool stunImmune = false;
    private float stunImmuneTimer = 0f;


    // Sprite / Animation
    private bool stunSpr = false;
    private float stunSprTimer = 0f;
    public float hitSprChanger = 0f;
    private string hitDir = "L";
    private bool sprFlip = false;

    private bool standsprcont = false;
    private float chanstandtimer = 0f;
    private Sprite curstandspr;


    // Death
    private bool isDead = false;
    private float deadTimer = 5f;
    private float deathflicker = 0f;


    // Phase
    private float phaseTimer = 0f;
    private float modeShiftTimer = 0f;


    // Misc
    private float dT;


    private void Awake()
    {
        // Initialize global health using ScriptableObject value
        GlobalPlayerVars.EnemyMaxHealth = enemyData.maxHealth;
        GlobalPlayerVars.EnemyHealth = enemyData.maxHealth;
        GlobalPlayerVars.EnemyName = enemyData.name;
        GlobalPlayerVars.goldvalue = enemyData.baseGoldWorth * GlobalPlayerVars.coinMultiplay;
        curstandspr = enemyData.sprStandingStill;
        sprrend = GetComponent<SpriteRenderer>();
        enemyEff = GetComponent<EnemyEffectsHandler>();
        aS = GetComponent<AudioSource>();
    }
    void Start()
    {
        corePos = transform.position;
        if (enemyData.isSlippery)
        {
        HandleSlip();
        }
        Vector2 newVec = new Vector2(xChanger, yChanger);
        startPos = corePos + newVec;
        if (enemyData.isSlippery)
        {
            if (xChanger == -0.282f)
                StartDodge(Vector2.left);
            else
                StartDodge(Vector2.right);
        }
    }

    void FixedUpdate()
    {
        if ((enemyData.atkAgro / 100) >= Random.value && !isAtk && !stunned)
            Attack();
    }

    void Update()
    {
        if (GlobalPlayerVars.PlayerHealth <= 0)
        {
            chanstandtimer += Time.deltaTime;
            if (standsprcont && chanstandtimer >= enemyData.idlespeed)
            {
                aS.PlayOneShot(enemyData.soundCele1);
                chanstandtimer = 0f;
                standsprcont = false;
                curstandspr = enemyData.sprPlayerDeath1;
                SpriteChange(curstandspr);
            }
            if (!standsprcont && chanstandtimer >= enemyData.idlespeed)
            {
                aS.PlayOneShot(enemyData.soundCele2);
                chanstandtimer = 0f;
                standsprcont = true;
                curstandspr = enemyData.sprPlayerDeath2;
                SpriteChange(curstandspr);
            }
        }
        else
        {
        dT = Time.deltaTime;
        HandleIdle();
            if (enemyData.modeShift != null)
                HandleModeShift();
        if (isDead != true)
            {
            enemyEff.EffectCheck();
            if (GlobalPlayerVars.EnemyHealth <= 0)
            {
                isDead = true;
                GlobalPlayerVars.gold += Mathf.RoundToInt(GlobalPlayerVars.goldvalue);
                aS.PlayOneShot(enemyData.soundDeath);
            }
            if (sprFlip)
                sprrend.flipX = true;
            else
                sprrend.flipX = false;
            HandleHit();
            HandleAttack();
            HandleDodge();
            HandleStun();
        }
        else
        {
            HandleDeath();
        }
        }
    }

    void HandleAttack()
    {

        if (isAtk)
        {
            timerAtk += dT;
            if (timerAtk < atkWARN - parTime)
            {
                atkSoundCheck = false;
                if (atkChoose.playWarning && !soundcheck2)
                {
                    aS.PlayOneShot(atkChoose.warnAttack);
                    soundcheck2 = true;
                }
                SpriteChange(sprATKWARN);
            }
            else
            {
                soundcheck2 = false;
                if (!atkSoundCheck)
                {
                    aS.PlayOneShot(atkChoose.soundAttack);
                    atkSoundCheck = true;
                }
                if (!atkChoose.unparryable)
                    isparryable = true;
                else
                    isparryable = false;
                SpriteChange(sprATK);
            }
            if (timerAtk >= atkWARN && countdownAtk == 0 && nextAtk == null)
            {
                isAtk = false;
                stunTimer += atkChoose.postAtkDodgeStun;
                timerAtk = 0;
                SpriteChange(curstandspr);
                if (!atkChoose.unstunable)
                    stunable = true;
                if (!atkChoose.isntAtker)
                    SendScore(target2, atkChoose.atkType, atkDAMA);
            }
            else if (timerAtk >= atkWARN && countdownAtk != 0)
            {
                countdownAtk -= 1;
                timerAtk = 0;
                stunTimer += atkChoose.postAtkDodgeStun;
                if (!atkChoose.unstunable)
                    stunable = true;
                if (!atkChoose.isntAtker)
                    SendScore(target2, atkChoose.atkType, atkDAMA);
            }
            else if (timerAtk >= atkWARN && countdownAtk == 0 && nextAtk != null)
            {
                timerAtk = 0;
                stunTimer += atkChoose.postAtkDodgeStun;
                if (!atkChoose.unstunable)
                    stunable = true;
                if (!atkChoose.isntAtker)
                    SendScore(target2, atkChoose.atkType, atkDAMA);
                AttackDictate(nextAtk);
            }
        }
    }

    void HandleDodge()
    {
        stunTimer += dT;

        if (isDodging)
        {
            dodgeTimer += dT;

            if (dodgeTimer <= enemyData.dodgeTime / 2f)
            {
                if (!isAtk)
                SpriteChange(enemyData.sprDodge);
                transform.position = Vector2.MoveTowards(transform.position, dodgeTarget, (enemyData.dodgeDistance / (enemyData.dodgeTime / 2f)) * dT);
            }
            else if (dodgeTimer <= enemyData.dodgeTime)
            {
                if (!isAtk)
                SpriteChange(enemyData.sprDodge);
                transform.position = Vector2.MoveTowards(transform.position, startPos, (enemyData.dodgeDistance / (enemyData.dodgeTime / 2f)) * dT);
            }
            else
            {
                sprFlip = false;
                SpriteChange(curstandspr);
                isDodging = false;
                transform.position = startPos;
            }
        }
    }

    void StartDodge(Vector2 direction)
    {
        isDodging = true;
        GlobalPlayerVars.goldvalue -= 5f * GlobalPlayerVars.coinMultiplay;
        dodgeTimer = 0f;
        aS.PlayOneShot(enemyData.soundDodge);
        stunTimer = 0f;
        dodgeTarget = (Vector2)transform.position + direction * enemyData.dodgeDistance;
    }

    public void ReceiveScore(string score, float damage, List<string> effectlist)
    {
        Debug.Log(score);
        bool canDodge =
            !isDodging &&
            !stunned &&
            ((enemyData.dodgeStun + enemyData.dodgeTime) < stunTimer);

        // Only allow parry during exact parry window
        if (isAtk && isparryable && timerAtk >= atkWARN - parTime && timerAtk <= atkWARN)
        {
            if (score == "rage")
            {
            ParrySet();
            return; // stop further processing
            }
            else if (atkChoose.parryableR == true && score == "bodyR")
            {
            ParrySet();
            return; // stop further processing
            }
            else if (atkChoose.parryableUpR == true && score == "headR")
            {
            ParrySet();
            return; // stop further processing
            }
            else if (atkChoose.parryableUpL == true && score == "headL")
            {
            ParrySet();
            return; // stop further processing
            }
            else if (atkChoose.parryableL == true && score == "bodyL")
            {
            ParrySet();
            return; // stop further processing
            }
        }

        if (canDodge)
        {
            bool dodgeSuccess = false;

            if (score != "rage")
                dodgeSuccess = Random.value <= (enemyData.atkRedyPercent - GlobalPlayerVars.dodgingRageNullifier);
            else
                dodgeSuccess = Random.value <= enemyData.atkRageRedyPercent;

            if (dodgeSuccess)
            {
                if (score == "headL" || score == "bodyL" || score == "rage")
                    StartDodge(Vector2.right);

                if (score == "headR" || score == "bodyR")
                {
                    sprFlip = true;
                    StartDodge(Vector2.left);
                }

                if (enemyData.postDodgeAtker)
                {
                    Attack();
                }

                return; // VERY IMPORTANT — stop here so no damage is applied
            }
        }

        if (enemyData.isSlippery)
        {
            if ((yChanger == 0.282f && xChanger == -0.282f) && (score != "headL" && score != "rage"))
                return; // VERY IMPORTANT — stop here so no damage is applied
            if ((yChanger == 0.282f && xChanger == 0.282f) && (score != "headR" && score != "rage"))
                return; // VERY IMPORTANT — stop here so no damage is applied
            if ((yChanger == 0f && xChanger == -0.282f) && (score != "bodyL" && score != "rage"))
                return; // VERY IMPORTANT — stop here so no damage is applied
            if ((yChanger == 0f && xChanger == 0.282f) && (score != "bodyR" && score != "rage"))
                return; // VERY IMPORTANT — stop here so no damage is applied
        }

        if (stunable && !stunned && !stunImmune)
        {
            stunable = false;   // consume window
            stunned = true;
            GlobalPlayerVars.goldvalue += 3f * GlobalPlayerVars.coinMultiplay;
            aS.PlayOneShot(enemyData.soundStunned);
            stunnedTimer = 0f;
        }

        if (score == "headR")
        {
            enemyEff.ApplyEffectsBasic(effectlist);
            float dama = damage * enemyData.headDamageMultiplier;
            if (!enemyData.unharmableVoidStun)
            {
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + 10, 100);
            }
            else if (stunned)
            {
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + 10, 100);
            }
            else
            {
                SpawnHit(0);
            }
            hitDir = "HR";
        }
        else if (score == "headL")
        {
            enemyEff.ApplyEffectsBasic(effectlist);
            float dama = damage * enemyData.headDamageMultiplier;
            if (!enemyData.unharmableVoidStun)
            {
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + 10, 100);
            }
            else if (stunned)
            {
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + 10, 100);
            }
            else
            {
                SpawnHit(0);
            }
            hitDir = "HL";
        }
        else if (score == "bodyL")
        {
            enemyEff.ApplyEffectsBasic(effectlist);
            float dama = damage * enemyData.bodyDamageMultiplier;
            if (!enemyData.unharmableVoidStun)
            {
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + 10, 100);
            }
            else if (stunned)
            {
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + 10, 100);
            }
            else
            {
                SpawnHit(0);
            }
            hitDir = "BL";
        }
        else if (score == "bodyR")
        {
            enemyEff.ApplyEffectsBasic(effectlist);
            float dama = damage * enemyData.bodyDamageMultiplier;
            if (!enemyData.unharmableVoidStun)
            {
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + 10, 100);
            }
            else if (stunned)
            {
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + 10, 100);
            }
            else
            {
                SpawnHit(0);
            }
            hitDir = "BR";
        }
        else
        {
            hitDir = "BR";
            SpawnHit(((int)damage));
            GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
        }

        if (enemyData.postHitAtker)
        {
            Attack();
        }
        
        if (!enemyData.unharmableVoidStun)
        hitSprChanger = .16f;
        else if (stunned)
        hitSprChanger = .16f;

        if (Random.value >= .5f)
            aS.PlayOneShot(enemyData.soundHit1);
        else
            aS.PlayOneShot(enemyData.soundHit2);

        if (enemyData.isSlippery && !stunned)
        {
            HandleSlip();
            Vector2 newVec = new Vector2(xChanger, yChanger);
            startPos = corePos + newVec;
            if (xChanger == -0.282f)
                StartDodge(Vector2.left);
            else
                StartDodge(Vector2.right);
        }
    }

    public void ParrySet()
    {
            isAtk = false;
            timerAtk = 0f;
            stunned = true;
            aS.PlayOneShot(enemyData.soundStunned);
            stunnedTimer = 0f;
    }


    public void Attack()
    {
        //Get Atk Data
        int atkIndex = Random.Range( 0, listOfAttacks.Count);
        atkChoose = listOfAttacks[atkIndex];

        sprATKWARN = atkChoose.sprAttackWarning;
        sprATK = atkChoose.sprAttack;
        atkDAMA = atkChoose.atkDamage;
        parTime = atkChoose.parryTime;
        atkWARN = atkChoose.atkWarning;
        countdownAtk = atkChoose.howManyTime;
        nextAtk = atkChoose.nextAtk;
        isAtk = true;
    }
    public void AttackDictate(AtkScriptable diAtk)
    {
        //Get Atk Data
        atkChoose = diAtk;
        nextAtk = null;

        sprATKWARN = atkChoose.sprAttackWarning;
        sprATK = atkChoose.sprAttack;
        atkDAMA = atkChoose.atkDamage;
        parTime = atkChoose.parryTime;
        atkWARN = atkChoose.atkWarning;
        countdownAtk = atkChoose.howManyTime;
        nextAtk = atkChoose.nextAtk;
        isAtk = true;
    }

    public void SendScore(PlayerMovement target2, string atkType, float damage)
    {
        if (target2 != null)
            target2.ReceiveScore(atkType, damage, atkChoose.attackEffects);
        else
            Debug.LogWarning("Target is missing!");
    }
    public void SpriteChange(Sprite sprite)
    {
        if (sprrend.sprite != sprite)
            sprrend.sprite = sprite;
    }

    public void SpawnHit(int amount)
    {
        GameObject obj = Instantiate(damageTextPrefab, canvasTransform);
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;

        obj.GetComponent<DamageNumber>().Init(amount);
    }

    public void CanAtk()
    {
        isAtk = false;
        timerAtk = 0;
        nextAtk = null;
        countdownAtk = 0;
    }

    public void HandleSlip()
    {
        int randran = Random.Range(0, 4);
        if (randran == 0)
        {
            xChanger = -0.282f;
            yChanger = 0f;
        }
        else if (randran == 1)
        {
            xChanger = 0.282f;
            yChanger = 0.282f;
        }
        else if (randran == 2)
        {
            xChanger = -0.282f;
            yChanger = 0.282f;
        }
        else
        {
            xChanger = 0.282f;
            yChanger = 0f;
        }
    }

    public void HandleIdle()
    {
        chanstandtimer += dT;
        if (standsprcont && chanstandtimer >= enemyData.idlespeed)
        {
            chanstandtimer = 0f;
            standsprcont = false;
            curstandspr = enemyData.sprStandingStill;
            SpriteChange(curstandspr);
        }
        if (!standsprcont && chanstandtimer >= enemyData.idlespeed)
        {
            chanstandtimer = 0f;
            standsprcont = true;
            curstandspr = enemyData.sprStandingStill2;
            SpriteChange(curstandspr);
        }
    }

    public void HandleHit()
    {
        hitSprChanger -= dT;
            if (hitSprChanger >= 0f)
            {
                if (hitDir == "HL")
                {
                    if (!isAtk)
                    SpriteChange(enemyData.sprHeadHitL);
                }
                else if (hitDir == "HR")
                {
                    if (!isAtk)
                    SpriteChange(enemyData.sprHeadHitR);
                }
                else if (hitDir == "BL")
                {
                    if (!isAtk)
                    SpriteChange(enemyData.sprBodyHitL);
                }
                else
                {
                    if (!isAtk)
                    SpriteChange(enemyData.sprBodyHitR);
                }
            }
            if (hitSprChanger <= 0f && hitSprChanger >= -.04)
            {
                SpriteChange(curstandspr);
            }
    }

    public void HandleDeath()
    {
        if (phase == 0)
        {
            winScreen.SetActive(true);
        }
        else
        {
            phaseTimer += dT;
        }
        deadTimer -= dT;
        deathflicker += dT;
        SpriteChange(enemyData.sprDead);
        if (deathflicker >= .3f)
        {
            sprrend.enabled = !sprrend.enabled;
            deathflicker = 0f;
        }
        if (phaseTimer >= 3f)
        {
            sprrend.enabled = sprrend.enabled;
            deathflicker = 0f;
            BPC.changePhase(phase);
        }
    }

    public void HandleStun()
    {
        if (stunImmune)
        {
                stunImmuneTimer += dT;
            if (stunImmuneTimer >= 0.4f) // adjust time as needed
            {
                stunImmune = false;
                stunImmuneTimer = 0f;
            }
        }
        if (stunable)
        {
            stunableTimer += dT;
            if (stunableTimer >= enemyData.postAtkStunTime)
            {
                stunableTimer = 0f;
                stunable = false;
            }
        }
        if (stunned)
        {
            isDodging = false;
            isAtk = false;
            timerAtk = 0;
            sprFlip = false;
            stunnedTimer += dT;
            stunSprTimer += dT;
            if (stunnedTimer >= enemyData.stunnedTime)
            {
                stunnedTimer = 0f;
                stunned = false;
                // Start temporary immunity
                stunImmune = true;
                stunImmuneTimer = 0f;
                SpriteChange(curstandspr);
            }
            if (stunSprTimer >= 0.25f && !stunSpr)
            {
                SpriteChange(enemyData.sprStunned1);
                stunSpr = true;
                stunSprTimer = 0;
            }
            if (stunSprTimer >= 0.25f && stunSpr)
            {
                SpriteChange(enemyData.sprStunned2);
                stunSpr = false;
                stunSprTimer = 0;
            }
        }
    }
    public void HandleModeShift()
    {
        modeShiftTimer += dT;
        if (modeShiftTimer >= enemyData.modeShiftSpeed)
        {
            modeShiftTimer = 0f;
            enemyData = enemyData.modeShift;
        }
    }
}