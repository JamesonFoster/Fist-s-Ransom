using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtk : MonoBehaviour
{
    [Header("Enemy Connection")]
    public EnemyMovement target;
    private PlayerMovement plMove;

    [Header("Basic Attack Stats")]
    public bool aimUp = false; // Holding W aims punches upward
    private float attackTimer = 0f;
    public bool isAtking = false;

    private Vector2 startPos;
    private Vector2 attackPos;
    private SpriteRenderer sprrend;
    private bool upSprites = false;
    private bool rageSprites = false;
    [Header("Sprites")]
    public Sprite standingStill;
    public Sprite upAtkPart1;
    public Sprite upAtkPart2;
    

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
        // Aim up check
        aimUp = Input.GetKey(KeyCode.W);

        // Attack input (only if not already attacking and player is allowed to move)
        if (!isAtking && plMove.canMove && Input.GetKeyDown(KeyCode.Comma))
            AttackL();
        if (!isAtking && plMove.canMove && Input.GetKeyDown(KeyCode.Period))
            AttackR();
        if (!isAtking && plMove.canMove && Input.GetKeyDown(KeyCode.Slash) && GlobalPlayerVars.PlayerRage == 100)
            AttackRage();
        if (Input.GetKeyDown(KeyCode.L))
            useHeal();
        if (Input.GetKeyDown(KeyCode.K))
            useRage();

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
            }
            else if (attackTimer <= GlobalPlayerVars.atkCooldown)
            {
                if (rageSprites)
                {
                    rageSprites = false;
                    SendScore(target, "headR", GlobalPlayerVars.rageHeadAtk);
                }
                // Move back
                transform.position = Vector2.Lerp(attackPos, startPos, (attackTimer - halfAtk) / halfAtk);
                if (upSprites)
                SpriteChange(upAtkPart2);
            }
            else
            {
                // Attack finished
                transform.position = startPos;
                attackTimer = 0f;
                isAtking = false;
                plMove.canMove = true; // Unlock movement after attack
                SpriteChange(standingStill);
                sprrend.flipX = false;
            }
        }
    }

    public void AttackL()
    {
        isAtking = true;
        plMove.canMove = false; // Lock movement while attacking

        if (aimUp)
        {
            SendScore(target, "headL", GlobalPlayerVars.headAtkDama);
            upSprites = true;
        }
        else
        {
            SendScore(target, "bodyL", GlobalPlayerVars.bodyAtkDama);
            upSprites = false;
        }
    }

    public void AttackR()
    {
        isAtking = true;
        plMove.canMove = false; // Lock movement while attacking

        if (aimUp)
        {
            sprrend.flipX = true;
            SendScore(target, "headR", GlobalPlayerVars.headAtkDama);
            upSprites = true;
        }
        else
        {
            SendScore(target, "bodyR", GlobalPlayerVars.bodyAtkDama);
            upSprites = false;
        }
    }
    public void AttackRage()
    {
        isAtking = true;
        plMove.canMove = false; // Lock movement while attacking

        if (aimUp)
        {
            attackTimer -= GlobalPlayerVars.atkCooldown;
            rageSprites = true;
        }
        else
        {
            attackTimer -= GlobalPlayerVars.atkCooldown;
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
            target.ReceiveScore(dir, damage);
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
