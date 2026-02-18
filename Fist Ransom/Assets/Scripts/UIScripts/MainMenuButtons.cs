using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject winScreen;
    public void StartRun()
    {
        winScreen.SetActive(true);
    }
}
