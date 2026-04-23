using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuButtons : MonoBehaviour
{
    public GameObject winScreen;
    public UpgradeManager uD;
    public int ModeNumb = 0;
    public void StartRun()
    {
        GlobalPlayerVars.playerMode = ModeNumb;
        winScreen.SetActive(true);
    }
    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void MainMenu()
    {
        uD.ClearUpgrades();
        GlobalPlayerVars.PlayerHealth = 100f;
        SceneManager.LoadScene("TitleScrene");
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
