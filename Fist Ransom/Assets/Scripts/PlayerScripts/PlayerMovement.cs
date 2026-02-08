using UnityEditor.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Dodging Stats")]
    public float dodgeDistance = 5f; //Distance the Player moves when dodging
    public float dodgeTime = 0.4f; //The time it takes for a player to no longer be dodging
    public float dodgeStun = 0.1f; //The time after a complete dodge where a player can't dodge again
    public bool isDodging = false; //Controls if player is dodging
    private float dodgeTimer = 0f; //A tester value to test how long the player has been dodging
    private float stunTimer = 999f; //A tester value to test how long the player has been dodge stunned
    private Vector2 dodgeTarget; //The target position the player moves to when dodging
    private Vector2 startPos; //The starting posistion / the position the player returns to after dodging
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
        if (!isDodging && ((dodgeStun + dodgeTime) < stunTimer && !plAtk.isAtking)) //Allows dodging only if player isn't dodging or atk
        {                                                       //and if dodge stun isn't active
            if (Input.GetKeyDown(KeyCode.A))
                StartDodge(Vector2.left); 

            if (Input.GetKeyDown(KeyCode.D))
                StartDodge(Vector2.right);

            if (Input.GetKeyDown(KeyCode.S))
                StartDodge(Vector2.down);
        }
        else
        {
            dodgeTimer += Time.deltaTime;
            
            if (dodgeTimer <= dodgeTime / 2f)
            {
                // Move outward
                transform.position = Vector2.MoveTowards(transform.position,dodgeTarget,(dodgeDistance / (dodgeTime / 2f)) * Time.deltaTime);
            }
            else if (dodgeTimer <= dodgeTime)
            {
                // Move back
                transform.position = Vector2.MoveTowards(transform.position,startPos,(dodgeDistance / (dodgeTime / 2f)) * Time.deltaTime);
            }
            else
            {
                isDodging = false;
                transform.position = startPos; // snap cleanly
            }
        }
    }

    void StartDodge(Vector2 direction) // A function used to set up the values for dodging
    {
        isDodging = true;
        dodgeTimer = 0f;
        stunTimer = 0f;
        dodgeTarget = (Vector2)transform.position + direction * dodgeDistance;
    }

    public void ReceiveScore(string score, float damage)
    {
        if (isDodging == false)
        {
            GlobalPlayerVars.PlayerHealth -= damage;
        }
    }

    public void SpriteChange(Sprite sprite)
    {
        sprrend.sprite = sprite;
    }
}
