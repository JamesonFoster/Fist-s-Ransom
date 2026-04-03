using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public bool isHeat = false;
    [SerializeField] private Slider _slider;
    void Start()
    {
        if (!isHeat)
        {
        _slider = GetComponent<Slider>();
        float healpercent = (GlobalPlayerVars.EnemyHealth / GlobalPlayerVars.EnemyMaxHealth) * 100;
        _slider.value = healpercent;
        }
        else
        {
        _slider = GetComponent<Slider>();
        float healpercent = GlobalPlayerVars.heatVal;
        _slider.value = healpercent;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHeat)
        {
        float healpercent = (GlobalPlayerVars.EnemyHealth / GlobalPlayerVars.EnemyMaxHealth) * 100;
        Debug.Log(healpercent);
        _slider.value = healpercent;
        }
        else
        {
        float healpercent = GlobalPlayerVars.heatVal;
        Debug.Log(healpercent);
        _slider.value = healpercent; 
        }
    }
}
