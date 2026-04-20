using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class Button4Upgrade : MonoBehaviour
{
    [Header("References")]
    public UpgradeManager upgradeManager; // your UpgradeManager
    public TextMeshProUGUI nameText;
    public Image iconImage;

    private Upgrade upgrade;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        // Auto-find UpgradeManager if not assigned
        if (upgradeManager == null)
            upgradeManager = FindObjectOfType<UpgradeManager>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    IEnumerator ReenableButton()
    {
        yield return new WaitForSeconds(0.1f);
        button.interactable = true;

    }

    public void SetUpgrade(Upgrade newUpgrade)
    {
        upgrade = newUpgrade;
        if (upgrade == null) return;

        if (nameText != null) nameText.text = upgrade.upgradeName;
        if (iconImage != null) iconImage.sprite = upgrade.icon;
    }

    public void OnClick()
    {
        if (upgrade == null || !button.interactable) return;

        button.interactable = false;
        upgradeManager.AddUpgrade(upgrade);
        StartCoroutine(ReenableButton());
    }
}