using UnityEngine;

public class RandomMoveAndFade : MonoBehaviour
{
    public float speed = 3f;
    public float fadeDuration = 2f;

    private Vector2 moveDirection;
    private SpriteRenderer spriteRenderer;
    private float fadeTimer;

    void Start()
    {
        // Get SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Generate random 2D direction
        moveDirection = Random.insideUnitCircle.normalized;

        fadeTimer = fadeDuration;
    }

    void Update()
    {
        // Move object
        transform.Translate(moveDirection * speed * Time.deltaTime);

        // Fade out
        if (fadeTimer > 0)
        {
            fadeTimer -= Time.deltaTime;
            float alpha = fadeTimer / fadeDuration;

            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}