using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopBGTrigger : MonoBehaviour
{
    private BossSpawner bossSpawner;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        bossSpawner = GetComponentInParent<BossSpawner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MainCamera"))
        {
            boxCollider.enabled = false;
            bossSpawner.StopScroll();
            Destroy(gameObject);
        }
    }
}
