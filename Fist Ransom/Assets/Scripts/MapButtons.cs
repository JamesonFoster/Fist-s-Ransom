using UnityEngine;
using UnityEngine.SceneManagement;

public class MapButtons : MonoBehaviour
{
    public int mapLocationID;
    public string locatType;
    public int accessablefromID1;
    public int accessablefromID2;
    private bool interactableButton = false;
    public GameObject playerOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (locatType == "")
        {
            int locatChoo = Random.Range(0, 3);
            if (locatChoo == 0)
                locatType = "enemy";
            else if (locatChoo == 1)
                locatType = "chest";
            else
                locatType = "miniboss";
        }
    }

    private void Start()
    {
        if (GlobalPlayerVars.playerLocationID == accessablefromID1 || GlobalPlayerVars.playerLocationID == accessablefromID2)
        {
            interactableButton = true;
        }
        else
            { interactableButton = false; }
        if (GlobalPlayerVars.playerLocationID == mapLocationID)
        {
            playerOn.SetActive(true);
        }
        else
        {
            playerOn.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClick()
    {
        if (interactableButton == true)
        {
            if (locatType == "enemy")
            {
                SceneManager.LoadScene("SampleScene");
            }
            else if (locatType == "chest")
            {
                SceneManager.LoadScene("BasicEnemyVic");
            }
        }
    }
}
