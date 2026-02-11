using UnityEngine;

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
    private SpriteRenderer sprrend;
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


    private void Awake()
    {
        // Initialize global health using ScriptableObject value
        GlobalPlayerVars.EnemyMaxHealth = enemyData.maxHealth;
        GlobalPlayerVars.EnemyHealth = enemyData.maxHealth;
        GlobalPlayerVars.EnemyName = enemyData.name;
        sprrend = GetComponent<SpriteRenderer>();
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
            }
            if (sprFlip)
                sprrend.flipX = true;
            else
                sprrend.flipX = false;
            hitSprChanger -= Time.deltaTime;
            if (hitSprChanger >= 0f)
            {
                if (hitDir == "L")
                {
                    SpriteChange(enemyData.sprHeadHitL);
                }
                else
                {
                    SpriteChange(enemyData.sprHeadHitR);
                }
            }
            if (hitSprChanger <= 0f && hitSprChanger >= -.01)
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
            if (timerAtk < enemyData.atkWarning - 0.1f)
            {
                SpriteChange(enemyData.sprAtkWarn);
            }
            else
            {
                SpriteChange(enemyData.sprAtkSwing);
            }
            if (timerAtk >= enemyData.atkWarning)
            {
                isAtk = false;
                timerAtk = 0;
                SpriteChange(enemyData.sprStandingStill);
                stunable = true;
                SendScore(target2, "normal", enemyData.atkDamage);
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
        stunTimer = 0f;
        dodgeTarget = (Vector2)transform.position + direction * enemyData.dodgeDistance;
    }

    public void ReceiveScore(string score, float damage)
    {
        if (Random.value <= enemyData.atkRedyPercent)
        {
            if (!isDodging && ((enemyData.dodgeStun + enemyData.dodgeTime) < stunTimer) && !stunned)
            {
                if (score == "headL" || score == "bodyL")
                    StartDodge(Vector2.right);

                if (score == "headR" || score == "bodyR")
                {
                    sprFlip = true;
                    StartDodge(Vector2.left);
                }
            }
        }
        else
        {
            GlobalPlayerVars.EnemyHealth -= damage;
            if (stunable)
            {
                stunable = false;
                stunned = true;
                stunableTimer = 0f;
            }
            if (GlobalPlayerVars.PlayerRage + 10 <= 100)
            {
                GlobalPlayerVars.PlayerRage += 10;
            }
            else
            {
                GlobalPlayerVars.PlayerRage = 100;
            }
            if (score == "headR")
            {
                hitDir = "R";
            }
            else
            {
                hitDir = "L";
            }
            hitSprChanger = .1f;
            Debug.Log($"Enemy Health: {GlobalPlayerVars.EnemyHealth}");
        }
    }

    public void Attack()
    {
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
