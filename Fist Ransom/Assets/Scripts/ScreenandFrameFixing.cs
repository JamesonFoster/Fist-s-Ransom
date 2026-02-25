using UnityEngine;

public class ScreenandFrameFixing : MonoBehaviour
{
    void Start()
    {
        // Limit FPS to 60
        Application.targetFrameRate = 60;

        // Optional: Make vsync off to ensure FPS limit works
        QualitySettings.vSyncCount = 0;
    }
}