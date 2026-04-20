using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DropDownSimController : MonoBehaviour
{
    [System.Serializable]
    public class EnemyInfo
    {
        public Sprite enemySprite;
        public string RoomString;
    }
    private TMP_Dropdown dD;
    public Image SpriteConnection;
    public string RoomName;
    private UpgradeManager upgradeManager;
    public List<EnemyInfo> listOEnemies;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        dD = GetComponent<TMP_Dropdown>();
        upgradeManager = FindObjectOfType<UpgradeManager>();
    }
    void Start()
    {
        DropDownChange();
    }

    public void DropDownChange()
    {
        RoomName = listOEnemies[dD.value].RoomString;
        SpriteConnection.sprite = listOEnemies[dD.value].enemySprite;
    }

    public void ButtonPress()
    {
        GlobalPlayerVars.playerAct = 0;
        GlobalPlayerVars.PlayerHealth = GlobalPlayerVars.PlayerMaxHealth;
        GlobalPlayerVars.PlayerRage = 0;
        GlobalPlayerVars.RageCount = 5;
        GlobalPlayerVars.HealCount = 5;
        upgradeManager.ClearUpgrades();
        SceneManager.LoadScene(RoomName);
    }
}
