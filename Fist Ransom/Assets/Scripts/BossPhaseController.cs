using UnityEngine;

public class BossPhaseController : MonoBehaviour
{
    public GameObject phase1;
    public GameObject phase2;
    public GameObject phase3;
    public PlayerAtk playerConnection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    } 

    public void changePhase(int phase)
    {
        phase1.SetActive(false);
        phase2.SetActive(false);
        phase3.SetActive(false);

        GameObject selectedPhase = null;

        if (phase == 2) selectedPhase = phase2;
        if (phase == 1) selectedPhase = phase3;

        if (selectedPhase != null)
        {
            EnemyMovement em = selectedPhase.GetComponent<EnemyMovement>();
            GlobalPlayerVars.EnemyHealth = em.enemyData.maxHealth;
            selectedPhase.SetActive(true);
            playerConnection.target = em;
        }
    }
}
