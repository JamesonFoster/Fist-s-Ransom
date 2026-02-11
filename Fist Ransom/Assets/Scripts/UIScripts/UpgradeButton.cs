using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Optional, auto-assigned if left empty")]
    public UpgradeManager upgradeManager;
    public TextMeshProUGUI descriptionText;

    private Upgrade upgrade;
    private Button button;
    private Image buttonImage;
    public GameObject winScreen;

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

        // Make sure button can receive clicks
        button.interactable = upgrade != null;
    }

    /// <summary>
    /// Assigns an Upgrade to this button
    /// </summary>
    public void SetUpgrade(Upgrade newUpgrade)
    {
        upgrade = newUpgrade;

        if (upgrade != null && buttonImage != null)
            buttonImage.sprite = upgrade.icon;

        // Enable button only if an upgrade is assigned
        if (button != null)
            button.interactable = (upgrade != null);
    }

    /// <summary>
    /// Called automatically when button is clicked
    /// </summary>
    public void OnClick()
    {
        if (upgrade == null || upgradeManager == null)
        {
            Debug.LogWarning("Click ignored: upgrade or manager missing.");
            return;
        }

        Debug.Log("Upgrade clicked: " + upgrade.upgradeName);

        upgradeManager.AddUpgrade(upgrade);

        // Disable all buttons after selection
        foreach (var ub in FindObjectsOfType<UpgradeButton>())
            ub.GetComponent<Button>().interactable = false;

        // Clear description text
        if (descriptionText != null)
            descriptionText.text = "";

        winScreen.SetActive(true);
    }

    /// <summary>
    /// Hover enters
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (upgrade != null && descriptionText != null)
            descriptionText.text = upgrade.description;
    }

    /// <summary>
    /// Hover exits
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (descriptionText != null)
            descriptionText.text = "";
    }
}
