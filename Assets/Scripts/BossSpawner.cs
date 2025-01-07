using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] private float initialWait = 5f;
    [SerializeField] private GameObject bossPrefab;
    private GameController gameController;
    [SerializeField] private bool stopScrollWhenBoss;
    [SerializeField] private ScrollingBackground[] backgrounds = new ScrollingBackground[3];
    [SerializeField] private BackgroundMover obstaclesBackgroundMover;

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
        if (stopScrollWhenBoss)
        {
            StartCoroutine(StopScroll());
        }
        yield return new WaitForSeconds(initialWait);       
        Instantiate(bossPrefab);
    }

    private IEnumerator StopScroll()
    {
        foreach (ScrollingBackground background in backgrounds)
        {
            background.scrollSpeed = 0;
        }
        obstaclesBackgroundMover.bgVelocity = 0;
        yield return null;
    }
}
