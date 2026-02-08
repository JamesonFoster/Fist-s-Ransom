using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHeathBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    private float _health = 100;
    // Start is called before the first frame update
    void Start()
    {
        float healpercent = (GlobalPlayerVars.PlayerMaxHealth / GlobalPlayerVars.PlayerHealth) * 100;
        _slider.value = healpercent;
    }

    // Update is called once per frame
    void Update()
    {
        float healpercent = (GlobalPlayerVars.PlayerMaxHealth / GlobalPlayerVars.PlayerHealth) * 100;
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (healpercent > 0)
            {
                healpercent -= 20f;
                _slider.value = healpercent;
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (healpercent <= 99)
            {
                healpercent += 20f;
                _slider.value = healpercent;
            }
        }
    }
}