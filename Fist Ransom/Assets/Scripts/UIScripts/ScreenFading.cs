using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(RawImage))]
public class ScreenFading : MonoBehaviour
{
    private RawImage screen;
    public float fadeDuration = 5f;
    public bool fadeIn = true;
    public string goTo;
    public bool back2Map = false;
    public bool dead = false;
    private GameObject onJe;
    private MusicPlayer musplay;

    void Awake()
    {
        onJe = GameObject.Find("MusicPlayer");
        musplay = onJe.GetComponent<MusicPlayer>();
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

        if (!string.IsNullOrEmpty(goTo))
        {
            SceneManager.LoadScene(goTo);
        }
        if (back2Map)
        {
            if (GlobalPlayerVars.playerAct == 1)
                SceneManager.LoadScene("Zone1Map");
            else if (GlobalPlayerVars.playerAct == 2)
                SceneManager.LoadScene("Zone2Map");
        }
        if (dead)
        {
            SceneManager.LoadScene("URDEAD");
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

        if (!string.IsNullOrEmpty(goTo))
        {
            SceneManager.LoadScene(goTo);
        }
        if (back2Map)
        {
            if (GlobalPlayerVars.playerAct == 1)
                SceneManager.LoadScene("Zone1Map");
            else if (GlobalPlayerVars.playerAct == 2)
                SceneManager.LoadScene("Zone2Map");
        }
        if (dead)
        {
            SceneManager.LoadScene("URDEAD");
        }
    }
}
