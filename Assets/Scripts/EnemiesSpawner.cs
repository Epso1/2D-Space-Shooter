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
    [SerializeField] private Transform parentTransform;
    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private bool instantiatesPowerUp = false;
    [SerializeField] private bool multipleVerticalOrigins = false;
    private Transform spawnPosition;
    public void SpawnEnemies()
    {
        StartCoroutine(SpawnEnemiesEnum());
    }

    private IEnumerator SpawnEnemiesEnum()
    {
        yield return new WaitForSeconds(initialWait);

        for (int i = 0; i < wavesQuantity; i++)
        {
            if (multipleVerticalOrigins)
            {
                spawnPosition = (i % 2 == 0) ? primaryPosition : secondaryPosition;
                // Instanciar enemigos de la oleada actual en posiciones verticales incrementales o decrementales
                Transform startingPosition = (i % 2 == 0) ? primaryPosition : secondaryPosition;
                int direction = (i % 2 == 0) ? 1 : -1; // Incrementar para pares, decrementar para impares

                for (int j = 0; j < enemiesQuantity; j++)
                {
                    Vector3 spawnPositionWithOffset = new Vector3(
                        startingPosition.position.x,
                        startingPosition.position.y + (j * direction), // Incrementar o decrementar posición en el eje Y para cada enemigo
                        startingPosition.position.z
                    );
                    Instantiate(enemyPrefab, spawnPositionWithOffset, Quaternion.identity, parentTransform);
                    yield return new WaitForSeconds(waitNextEnemy);
                }

                yield return new WaitForSeconds(primaryWaitNextWave);
            }
            else
            {
                spawnPosition = (i % 2 == 0) ? primaryPosition : secondaryPosition;

                // Instanciar enemigos de la oleada actual
                for (int j = 0; j < enemiesQuantity; j++)
                {
                    Instantiate(enemyPrefab, spawnPosition.position, Quaternion.identity, parentTransform);
                    yield return new WaitForSeconds(waitNextEnemy);
                }

                float waitTime = (i % 2 == 0) ? primaryWaitNextWave : secondaryWaitNextWave;
                yield return new WaitForSeconds(waitTime);
            }

            // Instanciar el power-up si corresponde
            if (instantiatesPowerUp && powerUpPrefab != null)
            {
                Instantiate(powerUpPrefab, spawnPosition.position, Quaternion.identity);
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
