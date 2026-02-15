using UnityEngine;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    public BaseEnemyScript enemyData; // Assign ScriptableObject in Inspector

    private bool isDodging = false;
    private float dodgeTimer = 0f;
    private float stunTimer = 999f;
    private Vector2 dodgeTarget;
    private Vector2 startPos;

    private float timerAtk = 0f;
    private bool isAtk = false;

    [Header("Connections")]
    public PlayerAtk target;
    public PlayerMovement target2;
    public GameObject winScreen;


    [Header("Attack List")]
    public List<AtkScriptable> listOfAttacks;


    //Private Code Stuff
    private SpriteRenderer sprrend;
    public AudioSource aS;
    private bool stunable = false;
    private float stunableTimer = 0f;
    private float stunnedTimer = 0f;
    private bool stunned = false;
    private bool stunSpr = false;
    private float stunSprTimer = 0f;
    private float hitSprChanger = 0f;
    private string hitDir = "L";
    private bool sprFlip = false;
    private bool isDead = false;
    private float deadTimer = 5f;
    private float deathflicker = 0f;
    private Sprite sprATKWARN;
    private Sprite sprATK;
    private float atkDAMA;
    private float parTime;
    private float atkWARN;
    private int countdownAtk;
    private AtkScriptable nextAtk;
    private AtkScriptable atkChoose;
    private bool atkSoundCheck;


    private void Awake()
    {
        // Initialize global health using ScriptableObject value
        GlobalPlayerVars.EnemyMaxHealth = enemyData.maxHealth;
        GlobalPlayerVars.EnemyHealth = enemyData.maxHealth;
        GlobalPlayerVars.EnemyName = enemyData.name;
        sprrend = GetComponent<SpriteRenderer>();
        aS = GetComponent<AudioSource>();
    }
    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (isDead != true)
            {
            if (GlobalPlayerVars.EnemyHealth <= 0)
            {
                isDead = true;
                aS.PlayOneShot(enemyData.soundDeath);
            }
            if (sprFlip)
                sprrend.flipX = true;
            else
                sprrend.flipX = false;
            hitSprChanger -= Time.deltaTime;
            if (hitSprChanger >= 0f)
            {
                if (hitDir == "HL")
                {
                    SpriteChange(enemyData.sprHeadHitL);
                }
                else if (hitDir == "HR")
                {
                    SpriteChange(enemyData.sprHeadHitR);
                }
                else if (hitDir == "BL")
                {
                    SpriteChange(enemyData.sprBodyHitL);
                }
                else
                {
                    SpriteChange(enemyData.sprBodyHitR);
                }
            }
            if (hitSprChanger <= 0f && hitSprChanger >= -.04)
            {
                SpriteChange(enemyData.sprStandingStill);
            }
            HandleAttack();
            HandleDodge();
            if (stunable)
            {
                stunableTimer += Time.deltaTime;
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
                sprFlip = false;

                stunnedTimer += Time.deltaTime;
                stunSprTimer += Time.deltaTime;
                if (stunnedTimer >= enemyData.stunnedTime)
                {
                    stunnedTimer = 0f;
                    stunned = false;
                    SpriteChange(enemyData.sprStandingStill);
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
        else
        {
            winScreen.SetActive(true);
            deadTimer -= Time.deltaTime;
            deathflicker += Time.deltaTime;
            SpriteChange(enemyData.sprDead);
            if (deathflicker >= .3f)
            {
                sprrend.enabled = !sprrend.enabled;
                deathflicker = 0f;
            }
        }
    }

    void HandleAttack()
    {
        if ((enemyData.atkAgro / 100) >= Random.value && !isAtk && !stunned)
            Attack();

        if (isAtk)
        {
            timerAtk += Time.deltaTime;
            if (timerAtk < atkWARN - parTime)
            {
                atkSoundCheck = false;
                SpriteChange(sprATKWARN);
            }
            else
            {
                if (!atkSoundCheck)
                {
                    aS.PlayOneShot(atkChoose.soundAttack);
                    atkSoundCheck = true;
                }
                SpriteChange(sprATK);
            }
            if (timerAtk >= atkWARN && countdownAtk == 0 && nextAtk == null)
            {
                isAtk = false;
                stunTimer += atkChoose.postAtkDodgeStun;
                timerAtk = 0;
                SpriteChange(enemyData.sprStandingStill);
                if (!atkChoose.unstunable)
                    stunable = true;
                SendScore(target2, "normal", atkDAMA);
            }
            else if (timerAtk >= atkWARN && countdownAtk == 0 && nextAtk != null)
            {
                timerAtk = 0;
                stunTimer += atkChoose.postAtkDodgeStun;
                if (!atkChoose.unstunable)
                    stunable = true;
                SendScore(target2, "normal", atkDAMA);
                AttackDictate(nextAtk);
            }
            else if (timerAtk >= atkWARN && countdownAtk != 0)
            {
                countdownAtk -= 1;
                timerAtk = 0;
                stunTimer += atkChoose.postAtkDodgeStun;
                if (!atkChoose.unstunable)
                    stunable = true;
                SendScore(target2, "normal", atkDAMA);
            }
        }
    }

    void HandleDodge()
    {
        stunTimer += Time.deltaTime;

        if (isDodging)
        {
            dodgeTimer += Time.deltaTime;

            if (dodgeTimer <= enemyData.dodgeTime / 2f)
            {
                SpriteChange(enemyData.sprDodge);
                transform.position = Vector2.MoveTowards(transform.position, dodgeTarget, (enemyData.dodgeDistance / (enemyData.dodgeTime / 2f)) * Time.deltaTime);
            }
            else if (dodgeTimer <= enemyData.dodgeTime)
            {
                SpriteChange(enemyData.sprDodge);
                transform.position = Vector2.MoveTowards(transform.position, startPos, (enemyData.dodgeDistance / (enemyData.dodgeTime / 2f)) * Time.deltaTime);
            }
            else
            {
                sprFlip = false;
                SpriteChange(enemyData.sprStandingStill);
                isDodging = false;
                transform.position = startPos;
            }
        }
    }

    void StartDodge(Vector2 direction)
    {
        isDodging = true;
        dodgeTimer = 0f;
        aS.PlayOneShot(enemyData.soundDodge);
        stunTimer = 0f;
        dodgeTarget = (Vector2)transform.position + direction * enemyData.dodgeDistance;
    }

    public void ReceiveScore(string score, float damage)
    {
        bool canDodge =
            !isDodging &&
            !stunned &&
            ((enemyData.dodgeStun + enemyData.dodgeTime) < stunTimer);

        if (canDodge)
        {
            bool dodgeSuccess = false;

            if (score != "headR" && score != "bodyR")
                dodgeSuccess = Random.value <= enemyData.atkRedyPercent;
            else
                dodgeSuccess = Random.value <= enemyData.atkRageRedyPercent;

            if (dodgeSuccess)
            {
                if (score == "headL" || score == "bodyL")
                    StartDodge(Vector2.right);

                if (score == "headR" || score == "bodyR")
                {
                    sprFlip = true;
                    StartDodge(Vector2.left);
                }

                return; // VERY IMPORTANT — stop here so no damage is applied
            }
        }

        // If we reach here → damage always applies
        GlobalPlayerVars.EnemyHealth -= damage;

        if (stunable)
        {
            stunable = false;
            stunned = true;
            stunableTimer = 0f;
        }

        GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + 10, 100);

        if (score == "headR")
            hitDir = "HR";
        else if (score == "headL")
            hitDir = "HL";
        else if (score == "bodyL")
            hitDir = "BL";
        else
            hitDir = "BR";

        hitSprChanger = .16f;

        if (Random.value >= .5f)
            aS.PlayOneShot(enemyData.soundHit1);
        else
            aS.PlayOneShot(enemyData.soundHit2);

        Debug.Log($"Enemy Health: {GlobalPlayerVars.EnemyHealth}");
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
            target2.ReceiveScore(atkType, damage);
        else
            Debug.LogWarning("Target is missing!");
    }
    public void SpriteChange(Sprite sprite)
    {
        sprrend.sprite = sprite;
    }
}
