using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private float initialWait;
    [SerializeField] private int enemiesQuantity;
    [SerializeField] private int wavesQuantity;
    [SerializeField] private float waitNextEnemy;
    [SerializeField] private float primaryWaitNextWave;
    [SerializeField] private float secondaryWaitNextWave;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform primaryPosition;
    [SerializeField] private Transform secondaryPosition;


    public void SpawnEnemies()
    {
        StartCoroutine(SpawnEnemiesEnum());
    }

    private IEnumerator SpawnEnemiesEnum()
    {
        yield return new WaitForSeconds(initialWait);
        for (int i = 0; i < wavesQuantity; i++)
        {
            if (i % 2 == 0)
            {
                for (int j = 0; j < enemiesQuantity; j++)
                {
                    Instantiate(enemyPrefab, primaryPosition.position, Quaternion.identity);
                    yield return new WaitForSeconds(waitNextEnemy);
                }
                yield return new WaitForSeconds(primaryWaitNextWave);
            }
            else
            {
                for (int j = 0; j < enemiesQuantity; j++)
                {
                    Instantiate(enemyPrefab, secondaryPosition.position, Quaternion.identity);
                    yield return new WaitForSeconds(waitNextEnemy);
                }
                yield return new WaitForSeconds(secondaryWaitNextWave);

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MainCamera"))
        {
            StartCoroutine(SpawnEnemiesEnum());
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
