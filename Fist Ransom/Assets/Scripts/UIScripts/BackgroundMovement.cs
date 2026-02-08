using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    public Vector2 moveTo;
    public float timer = 4f; // total time for there + back

    private Vector2 startingPos;
    private float t;

    void Start()
    {
        startingPos = transform.position;
    }

    void Update()
{
    // advance time (timer = full there + back duration)
    t += Time.deltaTime / (timer / 2f);

    // 0 → 1 → 0
    float lerpValue = Mathf.PingPong(t, 1f);

    // ease in & out
    float eased = Mathf.SmoothStep(0f, 1f, lerpValue);

    // apply movement
    transform.position = Vector2.Lerp(startingPos, moveTo, eased);
}
}
