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

    [Header("Player Connections")]
    public PlayerAtk target;
    public PlayerMovement target2;
    private SpriteRenderer sprrend;


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
        HandleAttack();
        HandleDodge();
    }

    void HandleAttack()
    {
        if ((enemyData.atkAgro / 100) >= Random.value && !isAtk)
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
                transform.position = Vector2.MoveTowards(transform.position, dodgeTarget, (enemyData.dodgeDistance / (enemyData.dodgeTime / 2f)) * Time.deltaTime);
            }
            else if (dodgeTimer <= enemyData.dodgeTime)
            {
                transform.position = Vector2.MoveTowards(transform.position, startPos, (enemyData.dodgeDistance / (enemyData.dodgeTime / 2f)) * Time.deltaTime);
            }
            else
            {
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
            if (!isDodging && ((enemyData.dodgeStun + enemyData.dodgeTime) < stunTimer))
            {
                if (score == "headL" || score == "bodyL")
                    StartDodge(Vector2.right);

                if (score == "headR" || score == "bodyR")
                    StartDodge(Vector2.left);
            }
        }
        else
        {
            GlobalPlayerVars.EnemyHealth -= damage;
            if (GlobalPlayerVars.PlayerRage + 10 <= 100)
            {
                GlobalPlayerVars.PlayerRage += 10;
            }
            else
            {
                GlobalPlayerVars.PlayerRage = 100;
            }
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
