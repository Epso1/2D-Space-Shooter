using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoaiPlayerDetector : MonoBehaviour
{
    private BossMoaiManager bossMoaiManager;
    [SerializeField] private float radius = 15f;
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        bossMoaiManager = GetComponentInParent<BossMoaiManager>();
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = radius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player detected!!!");
            circleCollider.enabled = false;
            bossMoaiManager.EnableColliders();
            bossMoaiManager.MoaisStartShooting();
            Destroy(gameObject);
        }
    }
}
