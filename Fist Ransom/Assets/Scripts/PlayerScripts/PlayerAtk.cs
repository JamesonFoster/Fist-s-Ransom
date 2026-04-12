using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAtk : MonoBehaviour
{
    [Header("Enemy Connection")]
    [SerializeField] public EnemyMovement target;
    private PlayerMovement plMove;
    public AudioSource aS;

    [Header("Basic Attack Stats")]
    public bool aimUp = false; // Holding W aims punches upward
    public float attackTimer = 0f;
    public bool isAtking = false;

    private Vector2 startPos;
    private bool hitStunned;
    private Vector2 attackPos;
    private SpriteRenderer sprrend;
    public float hitStunnedTimer;
    private bool upSprites = false;
    public bool rageSprites = false;
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


    [Header("Heat")]
    public GameObject sweatPrefab;
    public Vector3 sweatPos1;
    public Vector3 sweatPos2;
    private bool sweatPos = false;
    private float sweatTimer = 0f;


    private bool damageApplied = false;
    private string currentDir;
    private float currentDamage;
    private float heatHurt;
    
    void FixedUpdate()
    {
        if (GlobalPlayerVars.heatVal > 0f)
        GlobalPlayerVars.heatVal -= GlobalPlayerVars.heatDecreasingPer;
    }

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
        if (GlobalPlayerVars.heatVal >= 70f)
        {
            heatHurt = (GlobalPlayerVars.heatVal / 65f);
            sweatTimer += Time.deltaTime;
            if (sweatTimer > (1.5f - Mathf.InverseLerp(70f, 100f, GlobalPlayerVars.heatVal)))
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
        else
        {
            heatHurt = 1f;
        }

        if (!isAtking)
            plMove.canMove = true;

        if (hitStunnedTimer <= 0)
        {
            hitStunned = false;
            if (!plMove.isDodging)
                SpriteChange(standingStill);
        }
        else
        {
            hitStunned = true;
            if (!plMove.isDodging)
                SpriteChange(sprhitStunned);
        }
        // Aim up check
        aimUp = (Input.GetKey(KeyCode.W) || Input.GetAxis("Vertical") > 0.8f);

        if (hitStunned == false)
        {
            if (Gamepad.current != null)
            {
            // Attack input (only if not already attacking and player is allowed to move)
            if (!isAtking && !plMove.isSong && !plMove.dodgeAtkLock && plMove.canMove && (Input.GetKeyDown(KeyCode.Comma) || Input.GetKeyDown(KeyCode.Mouse0) || (Input.GetButtonDown("LeftAttack") || Gamepad.current.leftTrigger.ReadValue() > 0.1f)))
                AttackL();
            if (!isAtking && !plMove.isSong &&  !plMove.dodgeAtkLock && plMove.canMove && (Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.Mouse1) || (Input.GetButtonDown("RightAttack") || Gamepad.current.rightTrigger.ReadValue() > 0.1f)))
                AttackR();
            }
            else
            {
            // Attack input (only if not already attacking and player is allowed to move)
            if (!isAtking && !plMove.isSong && !plMove.dodgeAtkLock && plMove.canMove && (Input.GetKeyDown(KeyCode.Comma) || Input.GetKeyDown(KeyCode.Mouse0) || (Input.GetButtonDown("LeftAttack"))))
                AttackL();
            if (!isAtking && !plMove.isSong &&  !plMove.dodgeAtkLock && plMove.canMove && (Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.Mouse1) || (Input.GetButtonDown("RightAttack"))))
                AttackR();
            }
            if (!isAtking && !plMove.isSong && !plMove.dodgeAtkLock && plMove.canMove && (Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.Space) || (Input.GetButtonDown("RageAttack"))) && GlobalPlayerVars.PlayerRage == GlobalPlayerVars.PlayerRageMax)
                AttackRage(false);
            if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Q) || (Input.GetButtonDown("EatFood")))
                useHeal();
            if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.E) || (Input.GetButtonDown("EatAle")))
                useRage();
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.O))
            {
                SendScore(target, "bodyL", 200);
                SendScore(target, "bodyR", 200);
                SendScore(target, "headR", 200);
                SendScore(target, "headL", 200);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                GlobalPlayerVars.PlayerHealth -= 999;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                GlobalPlayerVars.PlayerRage = GlobalPlayerVars.PlayerRageMax;
            }
            #endif
        }

        // Attack movement
        if (isAtking)
        {
            attackTimer += Time.deltaTime;
            float halfAtk = (GlobalPlayerVars.atkCooldown * heatHurt) / 2f;

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
            else if (attackTimer <= (GlobalPlayerVars.atkCooldown * heatHurt))
            {
                // Apply damage ONCE when swing reaches halfway point
                if (!damageApplied)
                {
                    damageApplied = true;
                    SendScore(target, currentDir, currentDamage);
                    if (GlobalPlayerVars.heatVal < 100)
                        GlobalPlayerVars.heatVal += GlobalPlayerVars.heatPerHit;
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
    public void AttackRage(bool isagain)
    {
        isAtking = true;
        plMove.canMove = false;
        damageApplied = false;
        attackTimer = 0f;

        if (!isagain)
        {
        if (aimUp) 
        { 
            currentDir = "rageUp";
            attackTimer -= GlobalPlayerVars.PlayerRageSpeed; 
            currentDamage = GlobalPlayerVars.rageHeadAtk;
            rageSprites = true; 
        } 
        else 
        { 
            currentDir = "rageDown";
            attackTimer -= GlobalPlayerVars.PlayerRageSpeed; 
            currentDamage = GlobalPlayerVars.rageBodyAtk;
            rageSprites = true;
        }
        }
        else
        {
        if (aimUp) 
        { 
            currentDir = "rageUp2";
            attackTimer -= GlobalPlayerVars.PlayerRageSpeed / 2f; 
            currentDamage = GlobalPlayerVars.rageHeadAtk;
            rageSprites = true; 
        } 
        else 
        { 
            currentDir = "rageDown2";
            attackTimer -= GlobalPlayerVars.PlayerRageSpeed / 2f; 
            currentDamage = GlobalPlayerVars.rageBodyAtk;
            rageSprites = true;
        }  
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
            GlobalPlayerVars.heatVal -= GlobalPlayerVars.aleHeatDec;
            GlobalPlayerVars.RageCount--;
            GlobalPlayerVars.PlayerRage = Mathf.Min(GlobalPlayerVars.PlayerRage + GlobalPlayerVars.AleRageAmount, GlobalPlayerVars.PlayerRageMax);
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
