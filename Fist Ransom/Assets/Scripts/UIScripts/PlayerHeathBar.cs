using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHeathBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    void Start()
    {
        _slider = GetComponent<Slider>();
        float healpercent = (GlobalPlayerVars.PlayerHealth / GlobalPlayerVars.PlayerMaxHealth) * 100;
        _slider.value = healpercent;
    }

    // Update is called once per frame
    void Update()
    {
        float healpercent = (GlobalPlayerVars.PlayerHealth / GlobalPlayerVars.PlayerMaxHealth) * 100;
        _slider.value = healpercent;
    }
}