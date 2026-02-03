using UnityEngine;

public class PlayerHeathBar : MonoBehaviour
{
    private Vector3 cubStr;
    private Vector3 cubStrOg;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float healpercent = (GlobalPlayerVars.PlayerMaxHealth / GlobalPlayerVars.PlayerHealth) * 100;
        cubStrOg = transform.localScale;
        cubStr = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float healpercent = (GlobalPlayerVars.PlayerHealth / GlobalPlayerVars.PlayerMaxHealth);

        cubStr.x = cubStrOg.x * healpercent;

        transform.localScale = cubStr;
    }
}
