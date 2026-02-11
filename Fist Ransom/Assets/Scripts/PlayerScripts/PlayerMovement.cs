using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Dodging Stats")]
    public bool isDodging = false;

    [HideInInspector] public bool canMove = true; // Controls if player can move (used by attacks)

    private float dodgeTimer = 0f;
    private float stunTimer = 999f;
    private Vector2 dodgeTarget;
    private Vector2 startPos;

    private PlayerAtk plAtk;
    private SpriteRenderer sprrend;

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
                StartDodge(Vector2.left);
            if (Input.GetKeyDown(KeyCode.D))
                StartDodge(Vector2.right);
            if (Input.GetKeyDown(KeyCode.S))
                StartDodge(Vector2.down);
        }

        // Dodge movement
        if (isDodging)
        {
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
            GlobalPlayerVars.PlayerHealth -= damage;
        }
    }

    public void SpriteChange(Sprite sprite)
    {
        sprrend.sprite = sprite;
    }
}
