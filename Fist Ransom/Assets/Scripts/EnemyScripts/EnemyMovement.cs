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
    public GameObject sweatPrefab;
    public Vector3 sweatPos1;
    public Vector3 sweatPos2;
    private bool sweatPos = false;
    private float sweatTimer = 0f;
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
    private bool playerHasItem = false;
    private UpgradeManager upMan;
    private Vector2 attackOffset = Vector2.zero;
    private Vector2 dodgeOffset = Vector2.zero;
    private float dodgeRotation = 0f;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0f;
    private float shakeStart = 0f;
    private Vector2 shakeOffset = Vector2.zero;


    private void Awake()
    {
        // Initialize global health using ScriptableObject value
        if ((BPC == null && phase == 0) || (phase == 2 && BPC != null))
            GlobalPlayerVars.EnemyMaxHealth = enemyData.maxHealth;
        GlobalPlayerVars.EnemyHealth = enemyData.maxHealth;
        GlobalPlayerVars.EnemyName = enemyData.name;
        GlobalPlayerVars.goldvalue = enemyData.baseGoldWorth * GlobalPlayerVars.coinMultiplay;
        curstandspr = enemyData.sprStandingStill;
        sprrend = GetComponent<SpriteRenderer>();
        enemyEff = GetComponent<EnemyEffectsHandler>();
        aS = GetComponent<AudioSource>();
        upMan = FindObjectOfType<UpgradeManager>();
    }
    void Start()
    {
        corePos = transform.position;
        if (enemyData.atkNHitSettings.isSlippery)
        {
        HandleSlip();
        }
        Vector2 newVec = new Vector2(xChanger, yChanger);
        startPos = corePos + newVec;
        if (enemyData.atkNHitSettings.isSlippery)
        {
            if (xChanger == -0.282f)
            {
                sprFlip = true;
                StartDodge(Vector2.left);
            }
            else
                StartDodge(Vector2.right);
        }
        if (enemyData.itemTestingSettings.ifPlayerHas != null)
        {
            if (upMan.HasUpgrade(enemyData.itemTestingSettings.ifPlayerHas))
            playerHasItem = true;

            if (enemyData.itemTestingSettings.isModeShift && playerHasItem) //Mode Shifting
            enemyData = enemyData.itemTestingSettings.modeToShift2;
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
                if (enemyData.soundCele1 != null)
                aS.PlayOneShot(enemyData.soundCele1);
                chanstandtimer = 0f;
                standsprcont = false;
                curstandspr = enemyData.sprPlayerDeath1;
                SpriteChange(curstandspr);
            }
            if (!standsprcont && chanstandtimer >= enemyData.idlespeed)
            {
                if (enemyData.soundCele2 != null)
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
        if (!isAtk && !isDodging && !stunned)
            HandleIdle();
        if (enemyData.modeShiftSettings.modeShift != null)
            HandleModeShift();
        if (isDead != true)
        {
            enemyEff.EffectCheck();
            if (GlobalPlayerVars.EnemyHealth <= 0)
            {
                GlobalPlayerVars.heatVal = 0f;
                isDead = true;
                GlobalPlayerVars.gold += Mathf.RoundToInt(GlobalPlayerVars.goldvalue);
                aS.PlayOneShot(enemyData.soundDeath);
            }
            HandleHit();
            HandleDodge();
            HandleStun();
            HandleAttack();
            HandleShake();

            if ((BPC != null && phase == 1))
            {
                sweatTimer += dT;
                if (sweatTimer > 1f)
                {
                if (!sweatPos)
                    {
                        Instantiate(sweatPrefab, transform.position + sweatPos1, Quaternion.identity);
                        sweatTimer = 0f;
                        sweatPos = !sweatPos;    
                    }
                else
                    {
                        Instantiate(sweatPrefab, transform.position + sweatPos2, Quaternion.identity);
                        sweatTimer = 0f;
                        sweatPos = !sweatPos;
                    }
                }
            }
            if ((BPC != null && phase == 0))
            {
                sweatTimer += dT;
                if (sweatTimer > 0.5f)
                {
                    if (!sweatPos)
                    {
                        Instantiate(sweatPrefab, transform.position + sweatPos1, Quaternion.identity);
                        sweatTimer = 0f;
                        sweatPos = !sweatPos;    
                    }
                else
                    {
                        Instantiate(sweatPrefab, transform.position + sweatPos2, Quaternion.identity);
                        sweatTimer = 0f;
                        sweatPos = !sweatPos;
                    }
                }
            }

            // Handle All movement & Rotation
            transform.position = startPos + attackOffset + dodgeOffset + shakeOffset;
            transform.rotation = Quaternion.Euler(0f, 0f, dodgeRotation);
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
                    if (atkChoose.warnAttack != null)
                    aS.PlayOneShot(atkChoose.warnAttack);
                    soundcheck2 = true;
                }
                if (!atkChoose.unparryable && !atkChoose.noMoveBackWarning)
                {
                    float t = timerAtk / atkWARN;
                    attackOffset = Vector2.Lerp(Vector2.zero, new Vector2(0, 0.1f), t);
                }
                SpriteChange(sprATKWARN);
            }
            else
            {
                soundcheck2 = false;
                if (!atkSoundCheck)
                {
                    if (atkChoose.soundAttack != null)
                    aS.PlayOneShot(atkChoose.soundAttack);
                    atkSoundCheck = true;
                }
                if (!atkChoose.unparryable && !atkChoose.noMoveBackWarning)
                {
                    float t = timerAtk / atkWARN;
                    attackOffset = Vector2.Lerp(attackOffset, Vector2.zero, t);
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
                attackOffset = Vector2.zero;
                if (!atkChoose.unstunable)
                    stunable = true;
                if (!atkChoose.isntAtker)
                    SendScore(target2, atkChoose.atkType, atkDAMA);
                dodgeRotation = 0f;
            }
            else if (timerAtk >= atkWARN && countdownAtk != 0)
            {
                countdownAtk -= 1;
                timerAtk = 0;
                stunTimer += atkChoose.postAtkDodgeStun;
                attackOffset = Vector2.zero;
                if (!atkChoose.unstunable)
                    stunable = true;
                if (!atkChoose.isntAtker)
                    SendScore(target2, atkChoose.atkType, atkDAMA);
                dodgeRotation = 0f;
            }
            else if (timerAtk >= atkWARN && countdownAtk == 0 && nextAtk != null)
            {
                timerAtk = 0;
                stunTimer += atkChoose.postAtkDodgeStun;
                attackOffset = Vector2.zero;
                if (!atkChoose.unstunable)
                    stunable = true;
                if (!atkChoose.isntAtker)
                    SendScore(target2, atkChoose.atkType, atkDAMA);
                AttackDictate(nextAtk);
                dodgeRotation = 0f;
            }
        }
    }

    void HandleDodge()
    {
        stunTimer += dT;

        if (isDodging)
        {
            dodgeTimer += dT;
            float tiltDirection = -1f * Mathf.Sign(dodgeTarget.x - startPos.x);

            if (dodgeTimer <= enemyData.dodgeTime / 2f)
            {
                if (!isAtk)
                {
                    if (!sprFlip)
                    SpriteChange(enemyData.sprDodge);
                    else
                    SpriteChange(enemyData.sprDodgeL);
                }
                else
                {
                    dodgeRotation = Mathf.Lerp(0f, enemyData.dodgeAtkAngleIntence * tiltDirection, dodgeTimer / (enemyData.dodgeTime / 2f));
                }
                dodgeOffset = Vector2.MoveTowards(dodgeOffset, dodgeTarget - startPos, (enemyData.dodgeDistance / (enemyData.dodgeTime / 2f)) * dT);
            }
            else if (dodgeTimer <= enemyData.dodgeTime)
            {
                if (!isAtk)
                {
                    if (!sprFlip)
                    SpriteChange(enemyData.sprDodge);
                    else
                    SpriteChange(enemyData.sprDodgeL);
                }
                else
                {
                    dodgeRotation = Mathf.Lerp(enemyData.dodgeAtkAngleIntence * tiltDirection, 0f, (dodgeTimer - (enemyData.dodgeTime / 2f)) / (enemyData.dodgeTime / 2f));
                }
                dodgeOffset = Vector2.MoveTowards(dodgeOffset, Vector2.zero, (enemyData.dodgeDistance / (enemyData.dodgeTime / 2f)) * dT);
            }
            else
            {
                sprFlip = false;
                SpriteChange(curstandspr);
                isDodging = false;
                dodgeOffset = Vector2.zero;
                dodgeRotation = 0f;
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
        bool isRage = (score == "rageUp" || score == "rageDown" || score == "rageUp2" || score == "rageDown2");
        
        bool canDodge =
            !isDodging &&
            !stunned &&
            ((enemyData.dodgeStun + enemyData.dodgeTime) < stunTimer);

        // Only allow parry during exact parry window
        if (isAtk && isparryable && timerAtk >= atkWARN - parTime && timerAtk <= atkWARN)
        {
            if (isRage)
            {
            ParrySet();
            return;
            }
            else if (atkChoose.parryableR == true && score == "bodyR")
            {
            ParrySet();
            return;
            }
            else if (atkChoose.parryableUpR == true && score == "headR")
            {
            ParrySet();
            return;
            }
            else if (atkChoose.parryableUpL == true && score == "headL")
            {
            ParrySet();
            return;
            }
            else if (atkChoose.parryableL == true && score == "bodyL")
            {
            ParrySet();
            return;
            }
        }

        if (atkChoose != null)
        {
            //Handling Taughting Atks
            if ((score == "headL" || score == "bodyL" || isRage) && atkChoose.leftAtkResponse != null && isAtk)
            {
                timerAtk = 0;
                stunTimer += atkChoose.postAtkDodgeStun;
                attackOffset = Vector2.zero;
                if (!atkChoose.unstunable)
                    stunable = true;
                if (!atkChoose.isntAtker)
                    SendScore(target2, atkChoose.atkType, atkDAMA);
                dodgeRotation = 0f;
                AttackDictate(atkChoose.leftAtkResponse);
                return;
            }
            if ((score == "headR" || score == "bodyR") && atkChoose.rightAtkResponse != null && isAtk)
            {
                timerAtk = 0;
                stunTimer += atkChoose.postAtkDodgeStun;
                attackOffset = Vector2.zero;
                if (!atkChoose.unstunable)
                    stunable = true;
                if (!atkChoose.isntAtker)
                    SendScore(target2, atkChoose.atkType, atkDAMA);
                dodgeRotation = 0f;
                AttackDictate(atkChoose.rightAtkResponse);
                return;
            }
        }

        if (canDodge)
        {
            bool dodgeSuccess = false;

            if (!isRage)
                dodgeSuccess = Random.value <= enemyData.atkRedyPercent;
            else
                dodgeSuccess = Random.value <= (enemyData.atkRageRedyPercent - GlobalPlayerVars.dodgingRageNullifier);

            if (dodgeSuccess)
            {
                if (score == "headL" || score == "bodyL" || isRage)
                    StartDodge(Vector2.right);

                if (score == "headR" || score == "bodyR")
                {
                    if (!isAtk)
                    sprFlip = true;
                    StartDodge(Vector2.left);
                }

                if ((score == "rageUp" || score == "rageDown") && GlobalPlayerVars.scyllaAxe)
                {
                    target.AttackRage(true);
                }

                if (enemyData.atkNHitSettings.postDodgeAtker && !isAtk)
                {
                    Attack();
                }

                return;
            }
        }


        if (enemyData.atkNHitSettings.isSlippery)
        {
            if ((yChanger == 0.282f && xChanger == -0.282f) && (score != "headL" && !isRage))
                return;
            if ((yChanger == 0.282f && xChanger == 0.282f) && (score != "headR" && !isRage))
                return;
            if ((yChanger == 0f && xChanger == -0.282f) && (score != "bodyL" && !isRage))
                return;
            if ((yChanger == 0f && xChanger == 0.282f) && (score != "bodyR" && !isRage))
                return;
        }

        if (stunable && !stunned && !stunImmune)
        {
            stunable = false;
            stunned = true;
            GlobalPlayerVars.goldvalue += 3f * GlobalPlayerVars.coinMultiplay;
            aS.PlayOneShot(enemyData.soundStunned);
            stunnedTimer = 0f;
        }

        //Handles all damage calc
        HandleDamage(score, damage, effectlist);

        if (enemyData.atkNHitSettings.postHitAtker && !isAtk)
        {
            Attack();
        }
        
        if (!enemyData.atkNHitSettings.unharmableVoidStun)
        hitSprChanger = .16f;
        else if (stunned)
        hitSprChanger = .16f;

        if (Random.value >= .5f)
            aS.PlayOneShot(enemyData.soundHit1);
        else
            aS.PlayOneShot(enemyData.soundHit2);

        if (enemyData.atkNHitSettings.isSlippery && !stunned)
        {
            HandleSlip();
            Vector2 newVec = new Vector2(xChanger, yChanger);
            startPos = corePos + newVec;
            if (xChanger == -0.282f)
            {
                sprFlip = true;
                StartDodge(Vector2.left);
            }
            else
                StartDodge(Vector2.right);
        }
    }

    public void ParrySet()
    {
        isAtk = false;
        timerAtk = 0f;
        stunned = true;
        transform.position = startPos;
        aS.PlayOneShot(enemyData.soundStunned);
        stunnedTimer = 0f;
    }

    public void Attack()
    {
        //Get Atk Data if Not Empty
        if (listOfAttacks.Count != 0)
        {
            int atkIndex = Random.Range( 0, listOfAttacks.Count);
            atkChoose = listOfAttacks[atkIndex];
        }
        else if (enemyData.listOfSpAtks.Count != 0)
        {
            int atkIndex = Random.Range( 0, enemyData.listOfSpAtks.Count);
            atkChoose = enemyData.listOfSpAtks[atkIndex];
        }
        else
        {
            atkChoose = null;
        }

        if (atkChoose != null)
        {
        sprATKWARN = atkChoose.sprAttackWarning;
        sprATK = atkChoose.sprAttack;
        atkDAMA = atkChoose.atkDamage;
        parTime = atkChoose.parryTime / enemyData.atkSpeedMultiplier;
        atkWARN = atkChoose.atkWarning / enemyData.atkSpeedMultiplier;
        countdownAtk = atkChoose.howManyTime;
        nextAtk = atkChoose.nextAtk;
        if (atkChoose.isShake)
        {
            Quake(atkChoose.atkWarning, atkChoose.shakeMagna);
        }
        isAtk = true;
        }
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
                    if (shakeDuration <= 0)
                    Quake(0.1f,0.15f);
                }
                else if (hitDir == "HR")
                {
                    if (!isAtk)
                    SpriteChange(enemyData.sprHeadHitR);
                    if (shakeDuration <= 0)
                    Quake(0.1f,0.15f);
                }
                else if (hitDir == "BL")
                {
                    if (!isAtk)
                    SpriteChange(enemyData.sprBodyHitL);
                    if (shakeDuration <= 0)
                    Quake(0.1f,0.15f);
                }
                else
                {
                    if (!isAtk)
                    SpriteChange(enemyData.sprBodyHitR);
                    if (shakeDuration <= 0)
                    Quake(0.1f,0.15f);
                }
            }
            if (hitSprChanger <= 0f && hitSprChanger >= -.04)
            {
                if (!isAtk)
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
            if (stunImmuneTimer >= 0.4f)
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
            GlobalPlayerVars.heatVal -= GlobalPlayerVars.heatDecreasingPer;
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
        if (modeShiftTimer >= enemyData.modeShiftSettings.modeShiftSpeed && (!isAtk && !isDodging && !stunned))
        {
            modeShiftTimer = 0f;
            enemyData = enemyData.modeShiftSettings.modeShift;
        }
    }

    private void HandleDamage(string score, float damage, List<string> effectlist)
    {
        if (score == "headR") //Head Right Hit
        {
            enemyEff.ApplyEffectsBasic(effectlist);
            float dama = damage * enemyData.headDamageMultiplier;
            if (!enemyData.atkNHitSettings.unharmableVoidStun)
            {
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + GlobalPlayerVars.PlayerRagePerAtk, GlobalPlayerVars.PlayerRageMax);
            }
            else if (stunned)
            {
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + GlobalPlayerVars.PlayerRagePerAtk, GlobalPlayerVars.PlayerRageMax);
            }
            else
            {
                SpawnHit(0);
            }
            hitDir = "HR";
        }
        else if (score == "headL") //Head Left Hit
        {
            enemyEff.ApplyEffectsBasic(effectlist);
            float dama = damage * enemyData.headDamageMultiplier;
            if (!enemyData.atkNHitSettings.unharmableVoidStun)
            {
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + GlobalPlayerVars.PlayerRagePerAtk, GlobalPlayerVars.PlayerRageMax);
            }
            else if (stunned)
            {
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + GlobalPlayerVars.PlayerRagePerAtk, GlobalPlayerVars.PlayerRageMax);
            }
            else
            {
                SpawnHit(0);
            }
            hitDir = "HL";
        }
        else if (score == "bodyL") // Body Left Hit
        {
            enemyEff.ApplyEffectsBasic(effectlist);
            float dama = damage * enemyData.bodyDamageMultiplier;
            if (!enemyData.atkNHitSettings.unharmableVoidStun)
            {
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + GlobalPlayerVars.PlayerRagePerAtk, GlobalPlayerVars.PlayerRageMax);
            }
            else if (stunned)
            {
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + GlobalPlayerVars.PlayerRagePerAtk, GlobalPlayerVars.PlayerRageMax);
            }
            else
            {
                SpawnHit(0);
            }
            hitDir = "BL";
        }
        else if (score == "bodyR") // Body Right Hit
        {
            enemyEff.ApplyEffectsBasic(effectlist);
            float dama = damage * enemyData.bodyDamageMultiplier;
            if (!enemyData.atkNHitSettings.unharmableVoidStun)
            {
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + GlobalPlayerVars.PlayerRagePerAtk, GlobalPlayerVars.PlayerRageMax);
            }
            else if (stunned)
            {
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + GlobalPlayerVars.PlayerRagePerAtk, GlobalPlayerVars.PlayerRageMax);
            }
            else
            {
                SpawnHit(0);
            }
            hitDir = "BR";
        }
        else if (score == "rageUp" || score == "rageUp2") // Rage Up Hit
        {
            enemyEff.ApplyEffectsBasic(effectlist);
            if (!enemyData.atkNHitSettings.unharmableVoidStun)
            {
                float dama = damage * enemyData.headDamageMultiplier;
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + GlobalPlayerVars.PlayerRagePerAtk, GlobalPlayerVars.PlayerRageMax);
            }
            else if (stunned)
            {
                float dama = damage * enemyData.headDamageMultiplier;
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + GlobalPlayerVars.PlayerRagePerAtk, GlobalPlayerVars.PlayerRageMax);
            }
            else
            {
                float dama = damage;
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + GlobalPlayerVars.PlayerRagePerAtk, GlobalPlayerVars.PlayerRageMax);
            }
            hitDir = "BL";
        }
        else if (score == "rageDown" || score == "rageDown2") // Rage Down Hit
        {
            enemyEff.ApplyEffectsBasic(effectlist);
            if (!enemyData.atkNHitSettings.unharmableVoidStun)
            {
                float dama = damage * enemyData.bodyDamageMultiplier;
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + GlobalPlayerVars.PlayerRagePerAtk, GlobalPlayerVars.PlayerRageMax);
            }
            else if (stunned)
            {
                float dama = damage * enemyData.bodyDamageMultiplier;
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + GlobalPlayerVars.PlayerRagePerAtk, GlobalPlayerVars.PlayerRageMax);
            }
            else
            {
                float dama = damage;
                GlobalPlayerVars.EnemyHealth -= dama;
                SpawnHit(((int)dama));
                GlobalPlayerVars.goldvalue += 2f * GlobalPlayerVars.coinMultiplay;
                GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + GlobalPlayerVars.PlayerRagePerAtk, GlobalPlayerVars.PlayerRageMax);
            }
            hitDir = "BL";
        }
    }
    void HandleShake()
    {
        if (shakeDuration > 0)
        {
            float damper = shakeStart - shakeDuration;
            shakeOffset = Random.insideUnitCircle * shakeMagnitude * damper;
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeOffset = Vector2.zero;
        }
    }
    public void Quake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeStart = duration;
        shakeMagnitude = magnitude;
    }
}