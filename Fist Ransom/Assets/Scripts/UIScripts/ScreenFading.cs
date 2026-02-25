using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(RawImage))]
public class ScreenFading : MonoBehaviour
{
    private RawImage screen;
    public float fadeDuration = 5f;
    public bool isWin = true;
    public bool fadeIn = true;
    public bool back2Map = false;
    public bool toMainMenu = false;
    public bool toTutorial = false;
    private MusicPlayer musplay;

    void Awake()
    {
        musplay = GameObject.Find("MusicPlayer");
        screen = GetComponent<RawImage>();
    }

    void Start()
    {
        Color c = screen.color;

        // Set starting alpha depending on fade type
        c.a = fadeIn ? 0f : 1f;
        screen.color = c;
        musplay.SetVolume(1f - c.a);

        if (fadeIn)
            StartCoroutine(FadeIn());
        else
            StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        float time = 0f;
        Color c = screen.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            c.a = Mathf.Lerp(0f, 1f, t);
            screen.color = c;
            musplay.SetVolume(1f - c.a);

            yield return null;
        }

        c.a = 1f;
        screen.color = c;

        if (isWin)
            SceneManager.LoadScene("BasicEnemyVic");
        if (back2Map)
        {
            Debug.Log("LoadingBattle");
            SceneManager.LoadScene("Zone1Map");
        }
        if (toMainMenu)
        {
            Debug.Log("Going There");
            SceneManager.LoadScene("TitleScrene");
        }
        if (toTutorial)
        {
            SceneManager.LoadScene("Tutorial");
        }
    }

    IEnumerator FadeOut()
    {
        float time = 0f;
        Color c = screen.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            c.a = Mathf.Lerp(1f, 0f, t);
            screen.color = c;
            musplay.SetVolume(1f - c.a);

            yield return null;
        }

        c.a = 0f;
        screen.color = c;
        musplay.SetVolume(1f - c.a);

        if (isWin && !back2Map)
            SceneManager.LoadScene("BasicEnemyVic");
        if (back2Map)
        {
            SceneManager.LoadScene("Zone1Map");
        }
        if (toMainMenu)
        {
            Debug.Log("Going There");
            SceneManager.LoadScene("TitleScrene");
        }
        if (toTutorial)
        {
            SceneManager.LoadScene("Tutorial");
        }
    }
}
