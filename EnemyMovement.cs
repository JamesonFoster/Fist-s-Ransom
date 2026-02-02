using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float atkRedyPercent= 0.5f;
    public string score = "headL";

    public float dodgeDistance = 5f; //Distance the Player moves when dodging
    public float dodgeTime = 0.4f; //The time it takes for a player to no longer be dodging
    public float dodgeStun = 0.1f; //The time after a complete dodge where a player can't dodge again

    private bool isDodging = false; //Controls if player is dodging
    private float dodgeTimer = 0f; //A tester value to test how long the player has been dodging
    private float stunTimer = 999f; //A tester value to test how long the player has been dodge stunned
    private Vector2 dodgeTarget; //The target position the player moves to when dodging
    private Vector2 startPos; //The starting posistion / the position the player returns to after dodging


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        stunTimer += Time.deltaTime;

        if (isDodging == true)
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
    // Method to receive data from ScriptA
    public void ReceiveScore(string score)
    {
        if (Random.value <= atkRedyPercent)
        {
            string inatk = score;
            if (!isDodging && ((dodgeStun + dodgeTime) < stunTimer)) //Allows dodging only if player isn't dodging
            {                                                       //and if dodge stun isn't active
                if (inatk == "headL")
                {
                    StartDodge(Vector2.right);
                }
                if (inatk == "bodyL")
                {
                    StartDodge(Vector2.right);
                }

                if (inatk == "headR")
                {
                    StartDodge(Vector2.left);
                }
                if (inatk == "bodyR")
                {
                    StartDodge(Vector2.left);
                }
            }
        }
    }
}
