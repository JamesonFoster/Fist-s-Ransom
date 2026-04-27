using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonCodeControllerSupport : MonoBehaviour
{
    public int currentButtonCode;
    public bool isMap;

    [Header("Input Timing")]
    public float inputCooldown = 0.2f;
    private float lastMoveTime;

    private MapButtons[] buttons;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        if (isMap)
            RebuildAndSelect();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isMap)
            RebuildAndSelect();
    }

    void RebuildAndSelect()
    {
        buttons = FindObjectsOfType<MapButtons>(true); // include inactive just in case
        RefreshSelection();
    }

    public void RefreshSelection()
    {
        currentButtonCode = 0;

        foreach (var button in buttons)
        {
            if (button == null) continue;

            button.RefreshInteractable();

            if (button.interactableButton)
            {
                currentButtonCode = button.mapLocationID;
                break;
            }
        }
    }

    public bool CanMove()
    {
        return Time.time - lastMoveTime >= inputCooldown;
    }

    public void RegisterMove()
    {
        lastMoveTime = Time.time;
    }
}