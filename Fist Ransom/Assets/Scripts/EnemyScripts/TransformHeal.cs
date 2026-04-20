using UnityEngine;
using System.Collections.Generic;

public class TransformHeal : MonoBehaviour
{
    public float transformAtHeal = 50f;
    public List<BaseEnemyScript> listOfEnemies;
    public EnemyMovement connection;
    private bool hasChanged = false;

    // Update is called once per frame
    void Update()
    {
        if (GlobalPlayerVars.EnemyHealth <= transformAtHeal && !hasChanged)
        {
            int atkIndex = Random.Range( 0, listOfEnemies.Count);
            hasChanged = true;
            connection.enemyData = listOfEnemies[atkIndex];
        }
    }
}
