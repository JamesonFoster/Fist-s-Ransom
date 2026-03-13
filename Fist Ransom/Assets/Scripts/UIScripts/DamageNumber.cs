using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    public float speed = 50f;
    public float fadeDuration = 1f;

    private Vector2 moveDirection;
    private TextMeshProUGUI text;
    private float fadeTimer;
    private Color startColor;
    private int dama; 

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        startColor = text.color;
    }

    public void Init(int damageAmount)
    {
        text.text = damageAmount.ToString();
        dama = damageAmount;
        // Random 2D direction
        moveDirection = Random.insideUnitCircle.normalized;

        fadeTimer = fadeDuration;
    }

    void Update()
    {
        // Move in UI space
        transform.position += (Vector3)(moveDirection * speed * dama * Time.deltaTime);

        // Fade out
        if (fadeTimer > 0)
        {
            fadeTimer -= Time.deltaTime;
            float alpha = fadeTimer / fadeDuration;

            Color c = startColor;
            c.a = alpha;
            text.color = c;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}