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
    public float atkCooldown = 0.05f;
    private float timeratkCooldown = 0f;
    public bool isAtking = false;

    private Vector2 startPos;
    private Vector2 attackPos;
    [Header("Attack Damage Stats")]
    public float headAtkDama = 3;
    public float bodyAtkDama = 3;
    public float rageHeadAtk = 10;
    public float rageBodyAtk = 10;



    void Awake()
    {
        plMove = GetComponent<PlayerMovement>();
    }
    void Start()
    {
        startPos = transform.position;
        Vector2 attackPos = new Vector2(0, 0);
    }

    void Update()
    {
        // Aim up check
        aimUp = Input.GetKey(KeyCode.W);

        // Attack input
        if (!isAtking && timeratkCooldown <= 0f && !plMove.isDodging)
        {
            if (Input.GetKeyDown(KeyCode.Comma))
                AttackL();

            if (Input.GetKeyDown(KeyCode.Period))
                AttackR();
        }

        // Attack movement
        if (isAtking)
        {
            timeratkCooldown += Time.deltaTime;
            if (timeratkCooldown <= atkCooldown / 2f)
            {
                // Move outward
                transform.position = Vector2.MoveTowards(transform.position,attackPos,10f * Time.deltaTime);
            }
            else if (timeratkCooldown <= atkCooldown)
            {
                // Move back
                transform.position = Vector2.MoveTowards(transform.position,startPos,10f * Time.deltaTime);
            }
            else
            {
                // Reset
                isAtking = false;
                timeratkCooldown = 0f;
                transform.position = startPos;
            }
        }
    }

    public void AttackL()
    {
        isAtking = true;

        if (aimUp)
            SendScore(target, "headL", headAtkDama);
        else
            SendScore(target, "bodyL", bodyAtkDama);
    }

    public void AttackR()
    {
        isAtking = true;

        if (aimUp)
            SendScore(target, "headR", headAtkDama);
        else
            SendScore(target, "bodyR", bodyAtkDama);
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
    
}