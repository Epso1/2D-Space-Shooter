using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float minWaitToShoot = 0.2f;
    [SerializeField] private float maxWaitToShoot = 2f;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private int pointsWhenDies = 50;
    protected Transform player;
    protected Rigidbody2D rb;
    protected CircleCollider2D circleCollider;

    void Awake()
    {
   
    }

    // Cuando el enemigo no es visible, destruirlo
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
    // Cuando el enemigo es visible, activa su Collider
    private void OnBecameVisible()
    {
        circleCollider.enabled = true;
    }

    private void EnemyDies()
    {
        DataManager.Instance.AddPoints(pointsWhenDies);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // Dispara, después de un tiempo aleatorio, en la dirección del jugador
    private IEnumerator Shoot()
    {
        float randomWaitTime = Random.Range(minWaitToShoot, maxWaitToShoot);
        yield return new WaitForSeconds(randomWaitTime);
        if (player != null)
        {
            Vector3 shotDirection = (player.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, bulletOrigin.position, Quaternion.identity);
            bullet.GetComponent<EnemyBullet>().direction = shotDirection;
        }
        
    }

    protected void InitializeEnemy()
    {
        // Si se encuentra al jugador, guarda su Transform en la variable player para poder dispararle
        if (GameObject.FindWithTag("Player"))
        {
            player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }

        rb = GetComponent<Rigidbody2D>();

        // Guardamos el Collider para poder habilitarlo y deshabilitarlo
        circleCollider = GetComponent<CircleCollider2D>();

        // Deshabilita el Collider inicialmente para que no reciba disparos antes de ser visible
        circleCollider.enabled = false;

        // Inicia la corrutina de disparo        
        StartCoroutine(Shoot());
    }
}
