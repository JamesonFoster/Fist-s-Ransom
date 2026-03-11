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
    private SpriteRenderer sprrend;

    [Header("Possible Enemy Rooms")]
    public string[] enemyRooms;

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
            int locatChoo = Random.Range(1, 11);

            if (locatChoo <= 6)
                locatType = "enemy";
            else if (locatChoo == 7 || locatChoo == 8)
                locatType = "chest";
            else if (locatChoo >= 9)
                locatType = "shop";
            else
                locatType = "shop";
        }
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
    }

    void OnMouseDown()
    {
        if (GlobalPlayerVars.playerLocationID == accessablefromID1 ||
            GlobalPlayerVars.playerLocationID == accessablefromID2)
        {
            interactableButton = true;
        }
        else
        {
            interactableButton = false;
        }

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
                SceneManager.LoadScene("Boss1");
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
        }
    }
}
