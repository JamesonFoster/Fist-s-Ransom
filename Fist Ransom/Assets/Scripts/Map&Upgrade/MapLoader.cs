using UnityEngine;
using UnityEngine.SceneManagement;

public class MapLoader : MonoBehaviour
{
    public GameObject targetObject;
    private static MapLoader instance;
    public bool map1 = true;
    public bool map2 = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance != null && instance != this)
    {
        if (map2 && instance.map1)
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        return;
    }

    instance = this;
    DontDestroyOnLoad(gameObject);
    }   

    // Update is called once per frame
    void Update()
    {
        if (map1 == true)
        {
            GameObject found = GameObject.Find("MAP1DETECTOR");
            if (found)
            {
                GlobalPlayerVars.playerAct = 1;
                targetObject.SetActive(true);
            }
            else
            {
                targetObject.SetActive(false);
            }
            GameObject found2 = GameObject.Find("MAP2DETECTOR");
            if (found2)
            {
                Destroy(gameObject);
            }
        }
        if (map2 == true)
        {
            GameObject found = GameObject.Find("MAP2DETECTOR");
            if (found)
            {
                targetObject.SetActive(true);
                GlobalPlayerVars.playerAct = 2;
            }
            else
            {
                targetObject.SetActive(false);
            }
            GameObject found2 = GameObject.Find("MAP3DETECTOR");
            if (found2)
            {
                Destroy(gameObject);
            }
        }
    }
}
