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

    void Awake()
    {
        plAtk = GetComponent<PlayerAtk>();
        sprrend = GetComponent<SpriteRenderer>();
        originalColor = sprrend.color;
    }

    void Start()
    {
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        float regen = (GlobalPlayerVars.PlayerRegenPer / 5) * Time.deltaTime;
        if (GlobalPlayerVars.PlayerHealth > (GlobalPlayerVars.PlayerMaxHealth - regen))
        {
            GlobalPlayerVars.PlayerHealth += regen;
        }
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

        if (!isDodging)
            dodgeAtkLock = false;

        // Only allow dodging if player can move and dodge cooldown passed
        if (!isDodging && canMove && ((GlobalPlayerVars.dodgeStun + GlobalPlayerVars.dodgeTime) < stunTimer))
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                dodgeMode = 1;
                dodgeType = "left";
                StartDodge(Vector2.left);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                dodgeMode = 2;
                dodgeType = "right";
                StartDodge(Vector2.right);
            }
            if (Input.GetKeyDown(KeyCode.S))
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
            if ((Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.Space)) && GlobalPlayerVars.PlayerRage == 100)
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
    }

    public void takeDamage(float damage)
    {
        GlobalPlayerVars.PlayerHealth -= damage;
        GlobalPlayerVars.PlayerRage -= ((int)damage) * 2;
        plAtk.hitStunnedTimer = GlobalPlayerVars.hitStunnedLength;
    }

    public void SpriteChange(Sprite sprite)
    {
        sprrend.sprite = sprite;
    }

    public void HandleEffectApply(List<string> effects)
    {
        foreach (var eff in effects)
        {
            if (eff == "forDodgeL")
            {
                StartDodge(Vector2.left);
                dodgeSlower = 2f;
                dodgeAtkLock = true;
            }
            if (eff == "forDodgeR")
            {
                StartDodge(Vector2.right);
                dodgeSlower = 2f;
                dodgeAtkLock = true;
            }
            if (eff == "song")
            {
                isSong = true;
                colorLength = 7f;
                targetColor = new Color(1f, 0.4f, 0.7f);
                StartCoroutine(EffectFlicker());
            }
        }
    }

    public void HandleEffects()
    {
        if (isSong)
            Song();
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
}
