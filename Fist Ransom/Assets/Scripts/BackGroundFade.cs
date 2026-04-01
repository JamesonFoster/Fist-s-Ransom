using UnityEngine;

public class BackGroundFade : MonoBehaviour
{
    private SpriteRenderer screen;
    public GameObject targetEnemy;

    [Range(0f, 1f)] public float maxAlpha = 0.33f;
    public float fadeSpeed = 2f;

    void Start()
    {
        screen = GetComponent<SpriteRenderer>();

        Color c = screen.color;
        c.a = 0f;
        screen.color = c;
    }

    void Update()
    {
        // If no enemy or inactive → DO NOTHING (keep current alpha)
        if (targetEnemy == null || !targetEnemy.activeInHierarchy)
            return;

        if (GlobalPlayerVars.EnemyMaxHealth <= 0)
            return;

        Color c = screen.color;

        float healthPercent = GlobalPlayerVars.EnemyHealth / GlobalPlayerVars.EnemyMaxHealth;

        float targetAlpha = (1f - healthPercent) * maxAlpha;

        c.a = Mathf.MoveTowards(c.a, targetAlpha, Time.deltaTime * fadeSpeed);

        screen.color = c;
    }
}