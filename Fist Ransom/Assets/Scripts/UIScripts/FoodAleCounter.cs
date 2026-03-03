using UnityEngine;
using TMPro;

public class FoodAleCounter : MonoBehaviour
{
    public bool isAle = false;
    public bool isCoin = false;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        if (isAle == false && isCoin == false)
            textMeshPro.text = GlobalPlayerVars.HealCount.ToString();
        else if (isAle == true)
            textMeshPro.text = GlobalPlayerVars.RageCount.ToString();
        else
            textMeshPro.text = GlobalPlayerVars.gold.ToString();
    }
    void Update()
    {
        if (isAle == false && isCoin == false)
            textMeshPro.text = GlobalPlayerVars.HealCount.ToString();
        else if (isAle == true)
            textMeshPro.text = GlobalPlayerVars.RageCount.ToString();
        else
            textMeshPro.text = GlobalPlayerVars.gold.ToString();
    }
}