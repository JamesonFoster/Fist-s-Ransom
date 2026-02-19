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
        if (tStage == 1)
        {
            txt.text = "Press . to punch with your right fist.";
            if (Input.GetKeyDown(KeyCode.Period))
            {
                tStage = 2;
                keydot.SetActive(false);
                keyw.SetActive(true);
            }
        }
        if (tStage == 2)
        {
            txt.text = "Hold W to aim towards your enemies face!";
            if (Input.GetKeyDown(KeyCode.W))
            {
                tStage = 3;
                keyw.SetActive(false);
            }
        }
        if (tStage == 3)
        {
            txt.text = "Every time you land a punch your Rage Meter grows. Fill it.";
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                tStage = 4;
            }
        }
    }
}
