using UnityEngine;
using TMPro;

public class DioButton : MonoBehaviour
{
    [System.Serializable]
    public class DialogueEntry
    {
        public string text;
        public Sprite playerSprite;
        public Sprite enemySprite;
        public bool activateObject;
    }

    public DialogueEntry dialogue;

    public TextMeshProUGUI dialogueText;
    public SpriteRenderer Player;
    public SpriteRenderer Enemy;
    public GameObject activator;
    public GameObject otherButton;
    public int cost;

    public void ShowDialogue()
    {
        if (GlobalPlayerVars.gold >= cost)
        {
        GlobalPlayerVars.gold -= cost;

        if (dialogue == null) return;

        if (dialogue.playerSprite != null)
            Player.sprite = dialogue.playerSprite;

        if (dialogue.enemySprite != null)
            Enemy.sprite = dialogue.enemySprite;

        if (activator != null)
            activator.SetActive(dialogue.activateObject);

        otherButton.SetActive(false);
        gameObject.SetActive(false);

        if (dialogueText != null)
            dialogueText.text = dialogue.text;
        }
    }
}