using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAspectController : MonoBehaviour
{
    public float targetAspect = 16f / 9f; // Full HD aspect ratio

    void Start()
    {
        UpdateCamera();
    }

    void UpdateCamera()
    {
        Camera cam = GetComponent<Camera>();

        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            // Add black bars top & bottom (letterbox)
            Rect rect = cam.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            cam.rect = rect;
        }
        else
        {
            // Add black bars left & right (pillarbox)
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            cam.rect = rect;
        }
    }

    void Update()
    {
        // Optional: update if user resizes window
        UpdateCamera();
    }
}