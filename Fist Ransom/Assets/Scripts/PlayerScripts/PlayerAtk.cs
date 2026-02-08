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
    public float atkCooldown = 0.2f; // Slightly longer cooldown for visibility
    private float attackTimer = 0f;
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

        // Attack movement
        if (isAtking)
        {
            attackTimer += Time.deltaTime;
            float halfAtk = atkCooldown / 2f;

            if (attackTimer <= halfAtk)
            {
                // Move outward
                transform.position = Vector2.Lerp(startPos, attackPos, attackTimer / halfAtk);
            }
            else if (attackTimer <= atkCooldown)
            {
                // Move back
                transform.position = Vector2.Lerp(attackPos, startPos, (attackTimer - halfAtk) / halfAtk);
            }
            else
            {
                // Attack finished
                transform.position = startPos;
                attackTimer = 0f;
                isAtking = false;
                plMove.canMove = true; // Unlock movement after attack
            }
        }
    }

    public void AttackL()
    {
        isAtking = true;
        plMove.canMove = false; // Lock movement while attacking

        if (aimUp)
            SendScore(target, "headL", headAtkDama);
        else
            SendScore(target, "bodyL", bodyAtkDama);
    }

    public void AttackR()
    {
        isAtking = true;
        plMove.canMove = false; // Lock movement while attacking

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
