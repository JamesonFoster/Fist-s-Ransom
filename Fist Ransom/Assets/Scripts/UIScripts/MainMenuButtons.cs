using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuButtons : MonoBehaviour
{
    public GameObject winScreen;
    public void StartRun()
    {
        winScreen.SetActive(true);
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false; // stops play mode in editor
        #else
        Application.Quit(); // quits build
        #endif
    }
}
