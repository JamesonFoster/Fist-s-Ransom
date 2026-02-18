using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EndCutsceneGo : MonoBehaviour
{
    public VideoPlayer videoPlayer;   // Assign in Inspector
    public string titleSceneName = "Title"; // Name of your title scene

    void Start()
    {
        VideoPlayer vp = GetComponent<VideoPlayer>();
        vp.loopPointReached += OnVideoFinished;
    }


    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene("TitleScrene");
    }
}
