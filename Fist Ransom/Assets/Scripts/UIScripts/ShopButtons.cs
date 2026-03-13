using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class ShopButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Optional, auto-assigned if left empty")]
    public UpgradeManager upgradeManager;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;
    public GameObject soldOver;
    public float buttonEnableDelay = 1f;

    private Upgrade upgrade;
    private Button button;
    private Image buttonImage;
    private bool activated = true;


    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        // Auto-find description text if not assigned
        if (descriptionText == null)
        {
            descriptionText = FindObjectOfType<TextMeshProUGUI>();
            if (descriptionText == null)
                Debug.LogWarning("No TextMeshProUGUI found for descriptionText!");
        }

        // Auto-find UpgradeManager if not assigned
        if (upgradeManager == null)
        {
            upgradeManager = FindObjectOfType<UpgradeManager>();
            if (upgradeManager == null)
                Debug.LogError("No UpgradeManager found in scene!");
        }

        // Set up click listener automatically
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);

        button.interactable = false;
    }

    void Start()
    {
        StartCoroutine(EnableButtonDelay());
    }

    IEnumerator EnableButtonDelay()
    {
        yield return new WaitForSeconds(buttonEnableDelay);

        if (upgrade != null)
            button.interactable = true;
    }

    public void SetUpgrade(Upgrade newUpgrade)
    {
        upgrade = newUpgrade;

        costText.text = upgrade.Value.ToString();

        if (upgrade != null && buttonImage != null)
            buttonImage.sprite = upgrade.icon;
    }

    public void OnClick()
    {
        if (upgrade == null || upgradeManager == null || upgrade.Value > GlobalPlayerVars.gold || activated == false)
        {
            return;
        }

        Debug.Log("Upgrade clicked: " + upgrade.upgradeName);

        upgradeManager.AddUpgrade(upgrade);

        GlobalPlayerVars.gold -= upgrade.Value;

        // Disable this button
        activated = false;
        soldOver.SetActive(true);

        // Clear description text
        if (descriptionText != null)
            descriptionText.text = "";
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (upgrade != null && descriptionText != null)
            descriptionText.text = upgrade.description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (descriptionText != null)
            descriptionText.text = "";
    }
}

