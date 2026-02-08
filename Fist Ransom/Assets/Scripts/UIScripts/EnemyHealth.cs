using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    void Start()
    {
        _slider = GetComponent<Slider>();
        float healpercent = (GlobalPlayerVars.EnemyHealth / GlobalPlayerVars.EnemyMaxHealth) * 100;
        _slider.value = healpercent;
    }

    // Update is called once per frame
    void Update()
    {
        float healpercent = (GlobalPlayerVars.EnemyHealth / GlobalPlayerVars.EnemyMaxHealth) * 100;
        Debug.Log(healpercent);
        _slider.value = healpercent;
    }
}
