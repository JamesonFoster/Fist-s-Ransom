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

    //sprites
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
            int locatChoo = Random.Range(0, 10);

            if (locatChoo <= 5)
                locatType = "enemy";
            else if (locatChoo == 6)
                locatType = "chest";
            else if (locatChoo == 7)
                locatType = "miniboss";
            else
                locatType = "shop";
        }
    }

    void Start()
    {
        if (locatType == "enemy")
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

        if (locatType == "enemy")
        {
            GlobalPlayerVars.playerLocationID = mapLocationID;
            SceneManager.LoadScene("SampleScene");
        }
        else if (locatType == "chest")
        {
            GlobalPlayerVars.playerLocationID = mapLocationID;
            SceneManager.LoadScene("ChestReward");
        }
    }
}
