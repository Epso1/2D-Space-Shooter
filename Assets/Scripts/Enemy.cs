using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private bool flipHorizontally = false;
    [SerializeField] private bool followsPlayer = false;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private bool canShoot;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float minWaitToShoot = 0.5f;
    [SerializeField] private float maxWaitToShoot = 4f;
    [SerializeField] private Transform bulletOrigin;
    private Transform player;
    private Rigidbody2D rb;
    private Collider2D collider2D;
    private Animator animator;
    private Vector2 lastDirection = Vector2.left;

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
        if (canShoot == true)
        {
            StartCoroutine(Shoot());
        }
    }

    void FixedUpdate()
    {
        if (player != null && followsPlayer)
        {
            // Calcular la dirección hacia el jugador
            Vector2 direction = (player.position - transform.position).normalized;
            lastDirection = direction;

            // Calcular la nueva posición del enemigo
            Vector2 newPosition = rb.position + direction * speed * Time.fixedDeltaTime;

            // Mover el enemigo hacia la nueva posición
            rb.MovePosition(newPosition);
        }
        else if (player == null && followsPlayer)
        {
            Vector2 lastPosition = rb.position + lastDirection * speed * Time.fixedDeltaTime;
            rb.MovePosition(lastPosition);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet") || collision.CompareTag("Missile"))
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

    private IEnumerator Shoot()
    {
        float randomWaitTime = Random.Range(minWaitToShoot, maxWaitToShoot);
        yield return new WaitForSeconds(randomWaitTime);
        if (player != null)
        {
            Vector3 shotDirection = (player.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<EnemyBullet>().direction = shotDirection;
        }
        
    }
}
