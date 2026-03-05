using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Dodging Stats")]
    public bool isDodging = false;
    public string dodgeType;

    [HideInInspector] public bool canMove = true; // Controls if player can move (used by attacks)

    private float dodgeTimer = 0f;
    private float stunTimer = 999f;
    private int dodgeMode;
    private Vector2 dodgeTarget;
    private Vector2 startPos;

    private PlayerAtk plAtk;
    private SpriteRenderer sprrend;

    [Header("Sprites")]
    public Sprite standingStill;
    public Sprite dodgeBackSpr;
    public Sprite dodgeLeftSpr;
    public Sprite dodgeRightSpr;

    void Awake()
    {
        plAtk = GetComponent<PlayerAtk>();
        sprrend = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        stunTimer += Time.deltaTime;

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
            float halfDodge = GlobalPlayerVars.dodgeTime / 2f;

            if (dodgeTimer <= halfDodge)
            {
                transform.position = Vector2.MoveTowards(transform.position, dodgeTarget, (GlobalPlayerVars.dodgeDistance / halfDodge) * Time.deltaTime);
            }
            else if (dodgeTimer <= GlobalPlayerVars.dodgeTime)
            {
                transform.position = Vector2.MoveTowards(transform.position, startPos, (GlobalPlayerVars.dodgeDistance / halfDodge) * Time.deltaTime);
            }
            else
            {
                isDodging = false;
                SpriteChange(standingStill);
                transform.position = startPos;
            }
        }
    }

    void StartDodge(Vector2 direction)
    {
        isDodging = true;
        dodgeTimer = 0f;
        stunTimer = 0f;
        dodgeTarget = (Vector2)transform.position + direction * GlobalPlayerVars.dodgeDistance;
    }

    public void ReceiveScore(string score, float damage)
    {
        if (!isDodging)
        {
            takeDamage(damage);
        }
        else if (isDodging && score == "hitleft" && dodgeType == "left")
        {
            takeDamage(damage);
        }
        else if (isDodging && score == "hitright" && dodgeType == "right")
        {
            takeDamage(damage);
        }
        else if (isDodging && score == "hitdown" && dodgeType == "down")
        {
            takeDamage(damage);
        }
        else if (isDodging && score == "hitfullleft" && (dodgeType == "left" || dodgeType == "down"))
        {
            takeDamage(damage);
        }
        else if (isDodging && score == "hitfullright" && (dodgeType == "right" || dodgeType == "down"))
        {
            takeDamage(damage);
        }
        else if (isDodging && score == "hitfullsides" && (dodgeType == "right" || dodgeType == "left"))
        {
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
}
