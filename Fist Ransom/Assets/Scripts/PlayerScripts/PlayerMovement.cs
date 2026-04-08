using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    public GameObject loseFade;
    [Header("Dodging Stats")]
    public bool isDodging = false;
    public string dodgeType;
    public bool dodgeAtkLock = false;

    [HideInInspector] public bool canMove = true; // Controls if player can move (used by attacks)

    private float dodgeTimer = 0f;
    private float stunTimer = 999f;
    private int dodgeMode;
    private Vector2 dodgeTarget;
    private Vector2 startPos;
    private float dodgeSlower = 1f;

    private PlayerAtk plAtk;
    private SpriteRenderer sprrend;

    [Header("Sprites")]
    public Sprite standingStill;
    public Sprite dodgeBackSpr;
    public Sprite dodgeLeftSpr;
    public Sprite dodgeRightSpr;

    //Effects vals
    private float colorLength = 0.1f;
    private Color targetColor;
    private Color originalColor;
    //song vals
    public bool isSong = false;
    private float songTime = 7f;
    private float songTimer = 0f;
    //poison vals
    public bool isPoisoned = false;
    private float poisonTimer = 10f;
    private float poisonHitTimer = 0f;
    public Sprite posionVisual;
    public AudioClip soundPoisonHit;
    private CameraShake camShake;

    void Awake()
    {
        camShake = Camera.main.GetComponent<CameraShake>();
        plAtk = GetComponent<PlayerAtk>();
        sprrend = GetComponent<SpriteRenderer>();
        originalColor = sprrend.color;
    }

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (GlobalPlayerVars.PlayerHealth <= 0f)
        {
            loseFade.SetActive(true);
        }
        else
        {
            stunTimer += Time.deltaTime;
            HandleEffects();

            HandleRegen();

            if (!isDodging)
            {
                sprrend.color = new Color(sprrend.color.r, sprrend.color.g, sprrend.color.b, 1f);
                dodgeAtkLock = false;
            }
            else
            {
                sprrend.color = new Color(sprrend.color.r, sprrend.color.g, sprrend.color.b, 0.5f);
            }
            // Only allow dodging if player can move and dodge cooldown passed
            if (!isDodging && canMove && ((GlobalPlayerVars.dodgeStun + GlobalPlayerVars.dodgeTime) < stunTimer))
            {
                if ((Input.GetKeyDown(KeyCode.A) || Input.GetAxis("Horizontal") < -0.9f) && plAtk.hitStunnedTimer < 0f)
                {
                    dodgeMode = 1;
                    dodgeType = "left";
                    StartDodge(Vector2.left);
                }
                if ((Input.GetKeyDown(KeyCode.D) || Input.GetAxis("Horizontal") > 0.9f) && plAtk.hitStunnedTimer < 0f)
                {
                    dodgeMode = 2;
                    dodgeType = "right";
                    StartDodge(Vector2.right);
                }
                if ((Input.GetKeyDown(KeyCode.S) || Input.GetAxis("Vertical") < -0.9f) && plAtk.hitStunnedTimer < 0f)
                {
                    dodgeMode = 3;
                    dodgeType = "down";
                    StartDodge(Vector2.down);
                }
            }

            // Dodge movement
            if (isDodging)
            {
                if (dodgeMode == 1)
                    SpriteChange(dodgeLeftSpr);
                if (dodgeMode == 2)
                    SpriteChange(dodgeRightSpr);
                if (dodgeMode == 3)
                    SpriteChange(dodgeBackSpr);

                if ((Input.GetKeyDown(KeyCode.Comma) || Input.GetKeyDown(KeyCode.Mouse0)))
                    isDodging = false;
                if ((Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.Mouse1)))
                    isDodging = false;
                if ((Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.Space)) && GlobalPlayerVars.PlayerRage == GlobalPlayerVars.PlayerRageMax)
                    isDodging = false;

                dodgeTimer += Time.deltaTime;
                float halfDodge = (GlobalPlayerVars.dodgeTime / 2f) * dodgeSlower;

                if (dodgeTimer <= halfDodge)
                {
                    transform.position = Vector2.MoveTowards(transform.position, dodgeTarget, ((GlobalPlayerVars.dodgeDistance * dodgeSlower) / halfDodge) * Time.deltaTime);
                }
                else if (dodgeTimer <= (GlobalPlayerVars.dodgeTime * dodgeSlower))
                {
                    transform.position = Vector2.MoveTowards(transform.position, startPos, ((GlobalPlayerVars.dodgeDistance * dodgeSlower) / halfDodge) * Time.deltaTime);
                }
                else
                {
                    isDodging = false;
                    dodgeSlower = 1f;
                    SpriteChange(standingStill);
                    transform.position = startPos;
                }
            }
        }
    }

    void StartDodge(Vector2 direction)
    {
        isDodging = true;
        plAtk.isAtking = false;
        plAtk.attackTimer = 0f;
        dodgeTimer = 0f;
        stunTimer = 0f;
        dodgeTarget = (Vector2)transform.position + direction * GlobalPlayerVars.dodgeDistance;
        StartCoroutine(camShake.DirectionalShake(direction, 0.15f, GlobalPlayerVars.dodgeTime));
    }
    
    public void ReceiveScore(string score, float damage, List<string> effects)
    {
        if (!isDodging)
        {
            HandleEffectApply(effects);
            takeDamage(damage);
        }
        else if (isDodging && score == "hitleft" && dodgeType == "left")
        {
            HandleEffectApply(effects);
            takeDamage(damage);
        }
        else if (isDodging && score == "hitright" && dodgeType == "right")
        {
            HandleEffectApply(effects);
            takeDamage(damage);
        }
        else if (isDodging && score == "hitdown" && dodgeType == "down")
        {
            HandleEffectApply(effects);
            takeDamage(damage);
        }
        else if (isDodging && score == "hitfullleft" && (dodgeType == "left" || dodgeType == "down"))
        {
            HandleEffectApply(effects);
            takeDamage(damage);
        }
        else if (isDodging && score == "hitfullright" && (dodgeType == "right" || dodgeType == "down"))
        {
            HandleEffectApply(effects);
            takeDamage(damage);
        }
        else if (isDodging && score == "hitfullsides" && (dodgeType == "right" || dodgeType == "left"))
        {
            HandleEffectApply(effects);
            takeDamage(damage);
        }
        else
        {
            if (GlobalPlayerVars.heatVal > 0f)
                GlobalPlayerVars.heatVal -= GlobalPlayerVars.dodgeHeatDec;
        }
    }

    public void takeDamage(float damage)
    {
        GlobalPlayerVars.PlayerHealth -= damage;
        GlobalPlayerVars.PlayerRage -= ((int)damage) * 2;
        if (GlobalPlayerVars.scyllaCoat)
            GlobalPlayerVars.EnemyHealth -= (damage * 0.25f);
        plAtk.hitStunnedTimer = GlobalPlayerVars.hitStunnedLength;
        StartCoroutine(camShake.Shake(0.1f, 0.05f));
    }

    public void SpriteChange(Sprite sprite)
    {
        sprrend.sprite = sprite;
    }

    public void HandleEffectApply(List<string> effects)
    {
        foreach (var eff in effects)
        {
            if (GlobalPlayerVars.scyllaSoul && Random.value > 0.5f)
                    continue;
                    
            switch (eff)
            {
                case "forDodgeL":
                    StartDodge(Vector2.left);
                    dodgeSlower = 2f;
                    dodgeAtkLock = true;
                    break;
                
                case "forDodgeR":
                    StartDodge(Vector2.right);
                    dodgeSlower = 2f;
                    dodgeAtkLock = true;
                    break;
                
                case "song":
                    isSong = true;
                    colorLength = 7f;
                    targetColor = new Color(1f, 0.4f, 0.7f);
                    StartCoroutine(EffectFlicker());
                    break;
                
                case "poison":
                    isPoisoned = true;
                    colorLength = .32f;
                    targetColor = Color.green;
                    StartCoroutine(EffectFlicker());
                    break;

                case "RageDrain":
                    int rageLoss = Mathf.Max(1, GlobalPlayerVars.PlayerRage / 2);
                    GlobalPlayerVars.PlayerRage -= rageLoss;
                    colorLength = .32f;
                    targetColor = new Color(1f, 0.5f, 0.5f);
                    StartCoroutine(EffectFlicker());
                    break;
            }
        }
    }
    

    public void HandleRegen()
    {
        float regen = (GlobalPlayerVars.PlayerRegenPer / 5) * Time.deltaTime;
        if (GlobalPlayerVars.PlayerHealth < (GlobalPlayerVars.PlayerMaxHealth - regen))
        {
            GlobalPlayerVars.PlayerHealth += regen;
        }
    }

    public void CancelAll()
    {
        //Dodging Stops
        isDodging = false;
        dodgeSlower = 1f;
        SpriteChange(standingStill);
        transform.position = startPos;
        //Atk Stops
        plAtk.attackTimer = 0f;
        plAtk.isAtking = false;
        canMove = true;
        sprrend.flipX = false;
        plAtk.rageSprites = false;
    }

    public void HandleEffects()
    {
        if (isSong)
            Song();
        if (isPoisoned)
            Poison();
    }

    IEnumerator EffectFlicker()
    {
        sprrend.color = targetColor;
        yield return new WaitForSeconds(colorLength);
        sprrend.color = originalColor;
    }

    public void Song()
    {
        songTimer += Time.deltaTime;
        dodgeSlower = 2.5f;
        if (songTimer >= songTime)
        {
            songTimer = 0f;
            dodgeAtkLock = false;
            dodgeSlower = 1f;
            isSong = false;
        }
    }
    
    public void Poison()
    {
        poisonHitTimer += Time.deltaTime;
        poisonTimer -= Time.deltaTime;

        if (poisonTimer <= 0f)
        {
            poisonTimer = 10f;
            isPoisoned = false;
        }
        if (poisonHitTimer >= GlobalPlayerVars.poisonPlayerHitTimer)
        {
            Instantiate(posionVisual);
            plAtk.aS.PlayOneShot(soundPoisonHit);
            colorLength = .16f;
            targetColor = Color.green;
            StartCoroutine(EffectFlicker());
            GlobalPlayerVars.PlayerHealth -= 4f;
            CancelAll();
            poisonHitTimer = 0f;
        }
    }
}