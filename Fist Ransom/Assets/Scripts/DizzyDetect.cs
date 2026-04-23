using UnityEngine;

public class DizzyDetect : MonoBehaviour
{
    public GameObject dizzyEffect;

    // Update is called once per frame
    void Update()
    {
        if (GlobalPlayerVars.PlayerHealth <= 0f || GlobalPlayerVars.playerMode == 2)
        {
            Debug.Log("MODE 2");
            dizzyEffect.SetActive(true);
        }
    }
}
