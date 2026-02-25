using UnityEngine;

public class BackgroundRotate : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 50f;

    // Update is called once per frame
    void Update()
    {
        // Rotate around the Z-axis
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}