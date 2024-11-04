using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private bool flipHorizontally = false;
    [SerializeField] private bool followsPlayer = false;
    [SerializeField] private GameObject explosionPrefab;
    private Transform player;
    private Rigidbody2D rb;
    private Collider2D collider2D;
    private Animator animator;

    [SerializeField] private float speed = 2f;
    void Awake()
    {
        if(GameObject.FindWithTag("Player"))
        {
            player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }       
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        collider2D.enabled = false;
        if (flipHorizontally == true)
        {
            if (transform.position.y < 0)
            {
                animator.SetTrigger("FlipHorizontally");
            }
        }
        
    }

    void FixedUpdate()
    {
        if (player != null && followsPlayer)
        {
            // Calcular la dirección hacia el jugador
            Vector2 direction = (player.position - transform.position).normalized;

            // Calcular la nueva posición del enemigo
            Vector2 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;

            // Mover el enemigo hacia la nueva posición
            rb.MovePosition(newPosition);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player_Bullet"))
        {
            Destroy(collision.gameObject);
            EnemyDies();
        }
        else if (collision.CompareTag("Player"))
        {
            EnemyDies();
        }
    }
    private void OnBecameVisible()
    {
        collider2D.enabled = true;
    }

    private void EnemyDies()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
