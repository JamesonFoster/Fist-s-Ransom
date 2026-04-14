using UnityEngine;

public class OrbitUI : MonoBehaviour
{
    public float radius = 10f;
    public float speed = 1f;

    private Vector3 center;

    void Start()
    {
        center = transform.localPosition;
    }

    void Update()
    {
        float x = Mathf.Cos(Time.time * speed) * radius;
        float y = Mathf.Sin(Time.time * speed) * radius;

        transform.localPosition = center + new Vector3(x, y, 0);
    }
}