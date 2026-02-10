using UnityEngine;
using TMPro;

public class FoodAleCounter : MonoBehaviour
{
    public bool isAle = false;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        if (isAle == false)
            textMeshPro.text = GlobalPlayerVars.HealCount.ToString();
        else
            textMeshPro.text = GlobalPlayerVars.RageCount.ToString();
    }
    void Update()
    {
        if (isAle == false)
            textMeshPro.text = GlobalPlayerVars.HealCount.ToString();
        else
            textMeshPro.text = GlobalPlayerVars.RageCount.ToString();
    }
}