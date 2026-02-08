using UnityEngine;
using TMPro;

public class EnemyTitleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        textMeshPro.text = GlobalPlayerVars.EnemyName;
    }
}

