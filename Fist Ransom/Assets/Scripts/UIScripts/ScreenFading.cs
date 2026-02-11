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
    public bool back2Bat = false;

    void Awake()
    {
        screen = GetComponent<RawImage>();
    }

    void Start()
    {
        Color c = screen.color;

        // Set starting alpha depending on fade type
        c.a = fadeIn ? 0f : 1f;
        screen.color = c;

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

            yield return null;
        }

        c.a = 1f;
        screen.color = c;

        if (isWin)
            SceneManager.LoadScene("BasicEnemyVic");
        if (back2Bat)
        {
            Debug.Log("LoadingBattle");
            SceneManager.LoadScene("SampleScene");
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

            yield return null;
        }

        c.a = 0f;
        screen.color = c;

        if (isWin && !back2Bat)
            SceneManager.LoadScene("BasicEnemyVic");
        if (back2Bat)
        {
            Debug.Log("LoadingBattle");
            SceneManager.LoadScene("SampleScene");
        }
    }
}
