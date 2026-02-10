using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHeathBar : MonoBehaviour
{
    public bool isRageMeter = false;
    [SerializeField] private Slider _slider;
    void Start()
    {
        _slider = GetComponent<Slider>();
        if (isRageMeter == false)
        {
            float healpercent = (GlobalPlayerVars.PlayerHealth / GlobalPlayerVars.PlayerMaxHealth) * 100;
            _slider.value = healpercent;
        }
        else
        {
            float rage = GlobalPlayerVars.PlayerRage;
            _slider.value = rage;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isRageMeter == false)
        {
            float healpercent = (GlobalPlayerVars.PlayerHealth / GlobalPlayerVars.PlayerMaxHealth) * 100;
            _slider.value = healpercent;
        }
        else
        {
            float rage = GlobalPlayerVars.PlayerRage;
            _slider.value = rage;
        }
    }
}