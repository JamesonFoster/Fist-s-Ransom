using UnityEngine;

public class DontDestory : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // keeps this object across scenes
    }
}
