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
    public string ifGaut;
    public bool isItem;
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
        if (GlobalPlayerVars.playerAct == 0)
        {
            SceneManager.LoadScene("BattleSim");
        }

        if (!string.IsNullOrEmpty(goTo) && GlobalPlayerVars.playerAct != 0 && string.IsNullOrEmpty(ifGaut))
        {
            SceneManager.LoadScene(goTo);
        }
        else if (isItem && GlobalPlayerVars.playerMode == 3)
        {
            GlobalPlayerVars.eneKilled += 1;
            if (GlobalPlayerVars.eneKilled == 0)
            {
            SceneManager.LoadScene("SampleScene");
            }
            else if (GlobalPlayerVars.eneKilled == 1)
            {
            SceneManager.LoadScene("BigCrabFight");
            }
            else if (GlobalPlayerVars.eneKilled == 2)
            {
            SceneManager.LoadScene("SeaLionFight");
            }
            else if (GlobalPlayerVars.eneKilled == 3)
            {
            SceneManager.LoadScene("SirenFight");
            }
            else if (GlobalPlayerVars.eneKilled == 4)
            {
            SceneManager.LoadScene("Boss1");
            }
            else if (GlobalPlayerVars.eneKilled == 5)
            {
            SceneManager.LoadScene("GoblinGuyFight");
            }
            else if (GlobalPlayerVars.eneKilled == 6)
            {
            SceneManager.LoadScene("CyclopsFight");
            }
            else if (GlobalPlayerVars.eneKilled == 7)
            {
            SceneManager.LoadScene("SnakeGuyFight");
            }
            else if (GlobalPlayerVars.eneKilled == 8)
            {
            SceneManager.LoadScene("MedusaFight");
            }
            else if (GlobalPlayerVars.eneKilled == 9)
            {
            SceneManager.LoadScene("Boss2");
            }
        }
        else if (!string.IsNullOrEmpty(ifGaut) && GlobalPlayerVars.playerMode == 3)
        {
            SceneManager.LoadScene(ifGaut);
        }
        if (back2Map && GlobalPlayerVars.playerAct != 0 && GlobalPlayerVars.playerMode != 3)
        {
            if (GlobalPlayerVars.playerAct == 1)
                SceneManager.LoadScene("Zone1Map");
            else if (GlobalPlayerVars.playerAct == 2)
                SceneManager.LoadScene("Zone2Map");
        }
        if (dead && GlobalPlayerVars.playerAct != 0)
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

        if (!string.IsNullOrEmpty(goTo) && GlobalPlayerVars.playerAct != 0 && GlobalPlayerVars.playerMode != 3)
        {
            SceneManager.LoadScene(goTo);
        }
        if (!string.IsNullOrEmpty(ifGaut) && GlobalPlayerVars.playerMode == 3)
        {
            SceneManager.LoadScene(ifGaut);
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
