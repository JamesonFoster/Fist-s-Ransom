using UnityEngine;

public class BasicAnimator : MonoBehaviour
{
    [System.Serializable]
    public class AniVars
    {
        public Sprite sprite;
        public float time = 0.1f;
    }
    public AniVars[] frames;
    private int currentIndex = 0;
    private float timer = 0f;
    private SpriteRenderer sprrend;

    void Awake()
    {
        sprrend = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (frames.Length == 0) return;
        currentIndex = 0;
        sprrend.sprite = frames[currentIndex].sprite;
    }

    void Update()
    {
        if (frames.Length == 0) return;
        timer += Time.deltaTime;

        if (timer >= frames[currentIndex].time)
        {
            timer = 0f;

            currentIndex++;

            // Loop back to start
            if (currentIndex >= frames.Length)
                currentIndex = 0;

            sprrend.sprite = frames[currentIndex].sprite;
        }
    }
}