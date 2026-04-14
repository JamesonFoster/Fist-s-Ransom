using UnityEngine;

public class DizzyDetect : MonoBehaviour
{
    public GameObject dizzyEffect;

    // Update is called once per frame
    void Update()
    {
        if (GlobalPlayerVars.PlayerHealth <= 0f)
        {
            dizzyEffect.SetActive(true);
            Debug.Log("SetTrue");
        }
    }
}
