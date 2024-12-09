using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private float initialWait = 5f;
    [SerializeField] private GameObject bossPrefab;
    private GameController gameController;

    private void Awake()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MainCamera"))
        {
            SpawnBoss();
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    public void SpawnBoss()
    {
        StartCoroutine(SpawnBossEnum());
    }   
    

    private IEnumerator SpawnBossEnum()
    {
        yield return StartCoroutine(gameController.FadeOutMusicEnum());
        gameController.PlayBossMusic();
        yield return new WaitForSeconds(initialWait);       
        Instantiate(bossPrefab);
    }
}
