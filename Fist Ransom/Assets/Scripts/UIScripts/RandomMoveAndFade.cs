using UnityEngine;

public class RandomMoveAndFade : MonoBehaviour
{
    public float speed = 3f;
    public float fadeDuration = .5f;
    public bool floatDown = false;

    private Vector2 moveDirection;
    private SpriteRenderer spriteRenderer;
    private float fadeTimer;

    void Start()
    {
        // Get SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        fadeTimer = fadeDuration;
    }

    void Update()
    {
        // Move object
        if (!floatDown)
        {
        Vector2 moveDirection = Vector2.up;
        transform.Translate(moveDirection * speed * Time.deltaTime);
        }
        else
        {
        Vector2 moveDirection = Vector2.down;
        transform.Translate(moveDirection * speed * Time.deltaTime);   
        }

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