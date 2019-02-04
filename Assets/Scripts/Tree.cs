using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] int hitpoints = 5;
    [SerializeField] GameObject logPickupPrefab;
    [SerializeField] float logSpawnMaxDistance;

    public void TakeDamage(int damage)
    {
        hitpoints -= damage;
        if (hitpoints <= 0)
        {
            int amountOfLogs = Random.Range(2, 8);
            for (int i = 0; i < amountOfLogs; i++)
            {
                SpawnLog();
            }
            Destroy(gameObject);
        }
        else
        {
            if (Random.Range(1, 10) == 10)
            {
                SpawnLog();
            }
        }
    }

    void SpawnLog()
    {
        GameObject log = Instantiate(logPickupPrefab);
        log.transform.position = new Vector3(
            transform.position.x + Random.Range(-logSpawnMaxDistance, logSpawnMaxDistance),
            transform.position.y + Random.Range(-logSpawnMaxDistance, logSpawnMaxDistance),
            0.0f
        );
    }
}
