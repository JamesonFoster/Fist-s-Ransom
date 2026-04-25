using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MapButtons : MonoBehaviour
{
    public int mapLocationID;
    public string locatType;
    public int accessablefromID1;
    public int accessablefromID2;
    public bool interactableButton = false;
    public GameObject playerOn;
    private SpriteRenderer sprrend;

    [Header("Possible Enemy Rooms")]
    public string[] enemyRooms;

    [Header("Possible MiniBoss Rooms")]
    public string[] miniRooms;

    //sprites
    [Header("Sprites")]
    public Sprite enemyspr;
    public Sprite chestspr;
    public Sprite shopspr;
    public Sprite minibossspr;
    public Sprite bossspr;

    void Awake()
    {
        sprrend = GetComponent<SpriteRenderer>();
        if (string.IsNullOrEmpty(locatType))
        {
            int locatChoo = Random.Range(1, 19);

            if (locatChoo <= 13)
                locatType = "enemy";
            else if (locatChoo == 15 || locatChoo == 16)
                locatType = "chest";
            else if (locatChoo == 14)
                locatType = "shop";
            else
                locatType = "miniboss";
        }
    }
    public void RefreshInteractable()
    {
        interactableButton =
            GlobalPlayerVars.playerLocationID == accessablefromID1 ||
            GlobalPlayerVars.playerLocationID == accessablefromID2;
    }

    void OnEnable()
    {
        transform.localScale = new Vector3(0.2916f, 0.2916f, 1f);
    }

    void Start()
    {
        if (locatType == "enemy" || locatType == "firstenemy")
        sprrend.sprite = enemyspr;
        if (locatType == "chest")
        sprrend.sprite = chestspr;
        if (locatType == "miniboss")
        sprrend.sprite = minibossspr;
        if (locatType == "shop")
        sprrend.sprite = shopspr;
        if (locatType == "boss")
        sprrend.sprite = bossspr;
    }
    void Update()
    {
        // Show player marker
        if (GlobalPlayerVars.playerLocationID == mapLocationID)
            playerOn.SetActive(true);
        else
            playerOn.SetActive(false);
        
        if (!(GlobalPlayerVars.playerLocationID == accessablefromID1 ||
            GlobalPlayerVars.playerLocationID == accessablefromID2))
        {
            transform.localScale = new Vector3(0.2216f, 0.2216f, 1f);
        }
    }

    public void OnMouseDown()
    {
        #if UNITY_EDITOR
            interactableButton = true;
        #endif

        if (!interactableButton)
            return;

        switch(locatType)
        {
            case "enemy":
            {
                GlobalPlayerVars.playerLocationID = mapLocationID;

                int randRoom = Random.Range(0, enemyRooms.Length);
                string chosenRoom = enemyRooms[randRoom];

                SceneManager.LoadScene(chosenRoom);

                break;
            }
            case "chest":
            {
                GlobalPlayerVars.playerLocationID = mapLocationID;
                SceneManager.LoadScene("ChestReward");
                break;
            }
            case "boss":
            {
                GlobalPlayerVars.playerLocationID = mapLocationID;
                if (GlobalPlayerVars.playerAct == 1)
                    SceneManager.LoadScene("Boss1");
                else if (GlobalPlayerVars.playerAct == 2)
                    SceneManager.LoadScene("Boss2Talk");
                break;
            }
            case "shop":
            {
                GlobalPlayerVars.playerLocationID = mapLocationID;
                SceneManager.LoadScene("Shop");
                break;
            }
            case "firstenemy":
            {
                GlobalPlayerVars.playerLocationID = mapLocationID;
                SceneManager.LoadScene("SampleScene");
                break;
            }
            case "miniboss":
            {
                GlobalPlayerVars.playerLocationID = mapLocationID;

                int randRoom = Random.Range(0, miniRooms.Length);
                string chosenRoom = miniRooms[randRoom];

                SceneManager.LoadScene(chosenRoom);

                break;
            }
        }
    }

    void OnMouseEnter()
    {
        if (Gamepad.current == null)
        {
        if (GlobalPlayerVars.playerLocationID == accessablefromID1 ||
        GlobalPlayerVars.playerLocationID == accessablefromID2)
            transform.localScale = new Vector3(0.416f, 0.416f, 1f);
        }
    }

    void OnMouseExit()
    {
        if (Gamepad.current == null)
        {
        if (GlobalPlayerVars.playerLocationID == accessablefromID1 ||
            GlobalPlayerVars.playerLocationID == accessablefromID2)
            transform.localScale = new Vector3(0.2916f, 0.2916f, 1f);
        else
            transform.localScale = new Vector3(0.2216f, 0.2216f, 1f);
        }
    }
}
