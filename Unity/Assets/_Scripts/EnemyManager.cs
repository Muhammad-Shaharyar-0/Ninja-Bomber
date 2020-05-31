using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyManager : ScriptableObject {

    public List<GameObject> enemyTypes = new List<GameObject>();

    public List<Enemy> spawnedEnemies = new List<Enemy>();



    public void AddEnemy(Enemy enemy)
    {
        if (!spawnedEnemies.Contains(enemy))
        {
            spawnedEnemies.Add(enemy);
        }
    }


    public void RemoveEnemy(Enemy enemy)
    {
        if (spawnedEnemies.Contains(enemy))
        {
            spawnedEnemies.Remove(enemy);
        }
    }



}
