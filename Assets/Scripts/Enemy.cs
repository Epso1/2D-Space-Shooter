using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private bool followsPlayer = false;
    private Transform player;
    private Rigidbody2D rb;

    [SerializeField] private float speed = 2f;
    void Start()
    {

        if(GameObject.FindWithTag("Player"))
        {
            player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }       
        rb = GetComponent<Rigidbody2D>();
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
        if (collision.CompareTag("Player") || collision.CompareTag("Player_Bullet")) 
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
