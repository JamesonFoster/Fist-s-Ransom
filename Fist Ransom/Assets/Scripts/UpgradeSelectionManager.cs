using UnityEngine;

public class UpgradeSelectionManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform contentParent;          // ScrollView → Content
    public GameObject upgradeButtonPrefab;   // prefab with Button4Upgrade script

    [Header("Game References")]
    public UpgradeDatabase uD;               // your ScriptableObject database

    private void Start()
    {
        if (uD == null)
        {
            Debug.LogError("UpgradeDatabase not assigned!");
            return;
        }

        PopulateUpgradeButtons();
    }

    void PopulateUpgradeButtons()
    {
        foreach (var upgrade in uD.allUpgrades)  // assumes your database has List<Upgrade> allUpgrades
        {
            GameObject btnGO = Instantiate(upgradeButtonPrefab, contentParent);
            Button4Upgrade btn = btnGO.GetComponent<Button4Upgrade>();
            if (btn != null)
            {
                btn.SetUpgrade(upgrade);
            }
            else
            {
                Debug.LogError("Button4Upgrade script missing on prefab!");
            }
        }
    }
}