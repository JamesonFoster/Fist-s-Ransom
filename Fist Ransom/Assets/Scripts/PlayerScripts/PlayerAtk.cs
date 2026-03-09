using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtk : MonoBehaviour
{
    [Header("Enemy Connection")]
    [SerializeField] public EnemyMovement target;
    private PlayerMovement plMove;
    public AudioSource aS;

    [Header("Basic Attack Stats")]
    public bool aimUp = false; // Holding W aims punches upward
    private float attackTimer = 0f;
    public bool isAtking = false;

    private Vector2 startPos;
    private bool hitStunned;
    private Vector2 attackPos;
    private SpriteRenderer sprrend;
    public float hitStunnedTimer;
    private bool upSprites = false;
    private bool rageSprites = false;
    [Header("Sprites")]
    public Sprite standingStill;
    public Sprite upAtkPart1;
    public Sprite upAtkPart2;
    public Sprite bodyAtk1;
    public Sprite bodyAtk2;
    public Sprite sprhitStunned;
    public Sprite sprRageAtk1;
    public Sprite sprRageAtk2;
    [Header("Sounds")]
    public AudioClip rageSlash;




    private bool damageApplied = false;
    private string currentDir;
    private float currentDamage;
    

    void Awake()
    {
        plMove = GetComponent<PlayerMovement>();
        sprrend = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        startPos = transform.position;
        attackPos = startPos + new Vector2(0, 0.17f);
    }

    void Update()
    {
        hitStunnedTimer -= Time.deltaTime;

        if (hitStunnedTimer <= 0)
        {
            SpriteChange(standingStill);
            hitStunned = false;
        }
        else
        {
            hitStunned = true;
            SpriteChange(sprhitStunned);
        }
        // Aim up check
        aimUp = Input.GetKey(KeyCode.W);

        if (hitStunned == false)
        {
            // Attack input (only if not already attacking and player is allowed to move)
            if (!isAtking && plMove.canMove && (Input.GetKeyDown(KeyCode.Comma) || Input.GetKeyDown(KeyCode.Mouse0)))
                AttackL();
            if (!isAtking && plMove.canMove && (Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.Mouse1)))
                AttackR();
            if (!isAtking && plMove.canMove && (Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.Space)) && GlobalPlayerVars.PlayerRage == 100)
                AttackRage();
            if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Q))
                useHeal();
            if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.E))
                useRage();
        }

        // Attack movement
        if (isAtking)
        {
            attackTimer += Time.deltaTime;
            float halfAtk = GlobalPlayerVars.atkCooldown / 2f;

            if (attackTimer <= halfAtk)
            {
                // Move outward
                transform.position = Vector2.Lerp(startPos, attackPos, attackTimer / halfAtk);
                if (upSprites)
                SpriteChange(upAtkPart1);
                if (!upSprites && !rageSprites)
                SpriteChange(bodyAtk2);
                if (rageSprites)
                SpriteChange(sprRageAtk1);
            }
            else if (attackTimer <= GlobalPlayerVars.atkCooldown)
            {
                // Apply damage ONCE when swing reaches halfway point
                if (!damageApplied)
                {
                    damageApplied = true;
                    SendScore(target, currentDir, currentDamage);
                    if (rageSprites)
                    {
                        aS.PlayOneShot(rageSlash);
                    }
                }

                // Move back
                transform.position = Vector2.Lerp(
                    attackPos,
                    startPos,
                    (attackTimer - halfAtk) / halfAtk
                );

                if (upSprites)
                    SpriteChange(upAtkPart2);
                if (!upSprites && !rageSprites)
                    SpriteChange(bodyAtk1);
                if (rageSprites)
                SpriteChange(sprRageAtk2);
            }
            else
            {
                transform.position = startPos;
                attackTimer = 0f;
                isAtking = false;
                plMove.canMove = true;
                SpriteChange(standingStill);
                sprrend.flipX = false;
                rageSprites = false;
            }
        }
    }

    public void AttackR()
{
    isAtking = true;
    plMove.canMove = false;
    damageApplied = false;

    sprrend.flipX = true;

    if (aimUp)
    {
        currentDir = "headR";
        currentDamage = GlobalPlayerVars.headAtkDama;
        upSprites = true;
    }
    else
    {
        currentDir = "bodyR";
        currentDamage = GlobalPlayerVars.bodyAtkDama;
        upSprites = false;
    }
}

    public void AttackL()
{
    isAtking = true;
    plMove.canMove = false;
    damageApplied = false;

    sprrend.flipX = false;

    if (aimUp)
    {
        currentDir = "headL";
        currentDamage = GlobalPlayerVars.headAtkDama;
        upSprites = true;
    }
    else
    {
        currentDir = "bodyL";
        currentDamage = GlobalPlayerVars.bodyAtkDama;
        upSprites = false;
    }
}
    public void AttackRage()
    {
        currentDir = "rage";
        isAtking = true;
        plMove.canMove = false;
        damageApplied = false;
        attackTimer = 0f;

        if (aimUp) 
        { 
            attackTimer -= GlobalPlayerVars.PlayerRageSpeed; 
            currentDamage = GlobalPlayerVars.rageHeadAtk;
            rageSprites = true; 
        } 
        else 
        { 
            attackTimer -= GlobalPlayerVars.PlayerRageSpeed; 
            currentDamage = GlobalPlayerVars.rageBodyAtk;
            rageSprites = true;
        }
    }

    public void useHeal()
    {
        if (GlobalPlayerVars.HealCount > 0)
        {
            GlobalPlayerVars.HealCount--;
            if (GlobalPlayerVars.PlayerHealth <= (GlobalPlayerVars.PlayerMaxHealth / 4)*3)
            {
                GlobalPlayerVars.PlayerHealth += GlobalPlayerVars.PlayerMaxHealth / 4;
            }
            else
            {
                GlobalPlayerVars.PlayerHealth = GlobalPlayerVars.PlayerMaxHealth;
            }
        }
    }
    public void useRage()
    {
        if (GlobalPlayerVars.RageCount > 0)
        {
            GlobalPlayerVars.RageCount--;
            if (GlobalPlayerVars.PlayerRage <= 75)
            {
                GlobalPlayerVars.PlayerRage += 25;
            }
            else
            {
                GlobalPlayerVars.PlayerRage = 100;
            }
        }
    }


    public void SendScore(EnemyMovement target, string dir, float damage)
    {
        if (target != null)
        {
            target.ReceiveScore(dir, damage, GlobalPlayerVars.effectsList);
        }
        else
        {
            Debug.LogWarning("Target is missing!");
        }
    }
    public void SpriteChange(Sprite sprite)
    {
        sprrend.sprite = sprite;
    }
}
