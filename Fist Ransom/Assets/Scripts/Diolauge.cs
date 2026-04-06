using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Diolauge : MonoBehaviour
{
    [System.Serializable]
    public class DialogueEntry
    {
        public string text;
        public Sprite playerSprite;
        public Sprite enemySprite;
        public bool activateObject;
    }
    public List<DialogueEntry> dialogueList;
    public TextMeshProUGUI dialogueText;
    public SpriteRenderer Player;
    public SpriteRenderer Enemy;
    public GameObject activator;
    public int stage;
    // Update is called once per frame
    void Start()
    {
        ShowDialogue(stage);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && stage != dialogueList.Count - 1)
        {
            stage += 1;
            ShowDialogue(stage);
        }
    }
    void ShowDialogue(int stage)
    {
        DialogueEntry entry = dialogueList[stage];

        // Apply sprites
        if (entry.playerSprite != null)
            Player.sprite = entry.playerSprite;

        if (entry.enemySprite != null)
            Enemy.sprite = entry.enemySprite;

        // Activate object
        activator.SetActive(entry.activateObject);

        dialogueText.text = entry.text;
    }
}
