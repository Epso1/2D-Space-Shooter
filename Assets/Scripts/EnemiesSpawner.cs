using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private float initialWait;
    [SerializeField] private int enemiesQuantity;
    [SerializeField] private int wavesQuantity;
    [SerializeField] private float waitNextEnemy;
    [SerializeField] private float waitNextWave;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform initialPosition;

    void Start()
    {

    }

    void Update()
    {

    }

    public void SpawnEnemies()
    {
        StartCoroutine(SpawnEnemiesEnum());
    }

    private IEnumerator SpawnEnemiesEnum()
    {
        yield return new WaitForSeconds(initialWait);
        for (int i = 0; i < wavesQuantity; i++)
        {
            for (int j = 0; j < enemiesQuantity; j++)
            {
                Instantiate(enemyPrefab, initialPosition.position, Quaternion.identity);
                yield return new WaitForSeconds(waitNextEnemy);
            }

            yield return new WaitForSeconds(waitNextWave);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(SpawnEnemiesEnum());
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
