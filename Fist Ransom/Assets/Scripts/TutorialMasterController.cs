using UnityEngine;
using TMPro;

public class TutorialMasterController : MonoBehaviour
{
    public int tStage = 0;
    public TextMeshProUGUI txt;
    public GameObject keycomma;
    public GameObject keydot;
    public GameObject keyslash;
    public GameObject keyl;
    public GameObject keyk;
    public GameObject keyw;
    public GameObject keya;
    public GameObject keys;
    public GameObject keyd;


    public EnemyMovement enemyconn;
    public AtkScriptable atk1;
    public AtkScriptable atk2;
    public AtkScriptable atk3;
    // Update is called once per frame
    void Update()
    {
        if (tStage == 0)
        {
            txt.text = "Press , to punch with your left fist.";
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                tStage = 1;
                keycomma.SetActive(false);
                keydot.SetActive(true);
            }
        }
        else if (tStage == 1)
        {
            txt.text = "Press . to punch with your right fist.";
            if (Input.GetKeyDown(KeyCode.Period))
            {
                tStage = 2;
                keydot.SetActive(false);
                keyw.SetActive(true);
            }
        }
        else if (tStage == 2)
        {
            txt.text = "Hold W to aim towards your enemies face!";
            if (Input.GetKeyDown(KeyCode.W))
            {
                tStage = 3;
                keyw.SetActive(false);
            }
        }
        else if (tStage == 3)
        {
            txt.text = "Every time you land a punch your Rage Meter grows. Fill it.";
            if (GlobalPlayerVars.PlayerRage == 100)
            {
                tStage = 4;
                keyslash.SetActive(true);
            }
        }
        else if (tStage == 4)
        {
            txt.text = "Press / to use a Rage Cut.";
            if (Input.GetKeyDown(KeyCode.Slash))
            {
                tStage = 5;
                keyslash.SetActive(false);
                keya.SetActive(true);
            }
        }
        else if (tStage == 5)
        {
            if (!enemyconn.stunned)
            enemyconn.AttackDictate(atk1);
            txt.text = "Press A to dodge left.";
            if (Input.GetKeyDown(KeyCode.A))
            {
                tStage = 6;
                keya.SetActive(false);
                keyd.SetActive(true);
            }
        }
        else if (tStage == 6)
        {
            if (!enemyconn.stunned)
            enemyconn.AttackDictate(atk2);
            txt.text = "Press D to dodge right.";
            if (Input.GetKeyDown(KeyCode.D))
            {
                tStage = 89;
                keyd.SetActive(false);
                keys.SetActive(true);
            }
        }
        else if (tStage == 89)
        {
            if (!enemyconn.stunned)
            enemyconn.AttackDictate(atk3);
            txt.text = "Press S to dodge back.";
            if (Input.GetKeyDown(KeyCode.S))
            {
                tStage = 7;
                keys.SetActive(false);
                keyl.SetActive(true);
            }
        }
        else if (tStage == 7)
        {
            txt.text = "Press L to eat food.";
            if (Input.GetKeyDown(KeyCode.L))
            {
                tStage = 8;
                keyl.SetActive(false);
                keyk.SetActive(true);
            }
        }
        else if (tStage == 8)
        {
            txt.text = "Press K to drink ale.";
            if (Input.GetKeyDown(KeyCode.K))
            {
                tStage = 9;
                keyk.SetActive(false);
            }
        }
        else if (tStage == 9)
        {
            txt.text = "You are finished. Press Enter to leave.";
            if (Input.GetKeyDown(KeyCode.Return))
            {
            }
        }
    }
}
