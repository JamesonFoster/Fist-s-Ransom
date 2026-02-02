using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtk : MonoBehaviour
{
    public EnemyMovement target;
    
    public bool aimUp = false; //A tester value used to check if the player is hold W / aiming for up punches
    private string dir = "bodyL";


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) //If W is held check aimUp as true
        {
            aimUp = true;
            Debug.Log("W key is being held down.");
        }
        else
        {
            aimUp = false;
        }

        if (Input.GetKeyDown(KeyCode.Comma))
                AttackL(); 
        if (Input.GetKeyDown(KeyCode.Period))
                AttackR(); 

    }
    public void SendScore(EnemyMovement target, string dir)
    {
        if (target != null)
        {
            target.ReceiveScore(dir);
        }
        else
        {
            Debug.LogWarning("Target ScriptB is missing!");
        }
    }

    public void AttackL()
    {
        if (aimUp == true)
        {
            SendScore(target,"headL");
        }
        else
        {
            SendScore(target,"bodyL");
        }
    }
    public void AttackR()
    {
        if (aimUp == true)
        {
            SendScore(target,"headR");
        }
        else
        {
            SendScore(target,"bodyR");
        }
    }
}
