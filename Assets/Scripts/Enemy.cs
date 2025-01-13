using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] private bool isRandomShotTime;
    [SerializeField] private float minWaitToShoot = 0.2f;
    [SerializeField] private float maxWaitToShoot = 2f;
    [SerializeField] protected Transform bulletOrigin;
    [SerializeField] private int pointsWhenDies = 50;
    protected Transform player;
    protected Rigidbody2D rb;
    protected CircleCollider2D circleCollider;

    public string waveName;
    public int waveElementsQuantity;
    public GameObject powerUpToSpawn;
    public bool instantiatesPowerUp;

    private float enableColliderTime = 0.2f;

    void Awake()
    {
   
    }
    // Cuando el enemigo es visible, activa su Collider
    private void OnBecameVisible()
    {
        StartCoroutine(EnableCollider());
    }

    // Cuando el enemigo no es visible, destruirlo
    private void OnBecameInvisible()
    {
        DisableCollider();
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
   

    private void EnemyDies()
    {
        DisableCollider();
        DataManager.Instance.AddPoints(pointsWhenDies);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Instanciar el power-up si corresponde
        if (isTheLastStanding() && instantiatesPowerUp && powerUpToSpawn != null)
        {
            Instantiate(powerUpToSpawn, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    // Dispara, después de un tiempo aleatorio, en la dirección del jugador
    private IEnumerator Shoot()
    {
        if (isRandomShotTime)
        {
            float randomWaitTime = Random.Range(minWaitToShoot, maxWaitToShoot);
            yield return new WaitForSeconds(randomWaitTime);
        }
        
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
        
        if (isRandomShotTime)
        {
            // Inicia la corrutina de disparo        
            StartCoroutine(Shoot());
        }
        
    }

    private bool isTheLastStanding()
    {
        bool isLast = false;
        int destroyedCount = 0;
        if (DataManager.Instance.enemyWaves.ContainsKey(waveName))
        {
            foreach(GameObject enemy in DataManager.Instance.enemyWaves[waveName])
            {
                if (enemy == null){
                    destroyedCount++;
                    Debug.Log($"destroyedCount: {destroyedCount}.");
                }
            }
            if (destroyedCount >= (waveElementsQuantity - 1))
            {
                isLast = true;
            }
        }
        else
        {
            Debug.Log("Wave not found.");
        }
        return isLast;
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(enableColliderTime);
        circleCollider.enabled = true;
    }
    private void DisableCollider() 
    { 
        circleCollider.enabled = false;
    }
}
