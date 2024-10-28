using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Velocidad del misil
    [SerializeField] private Vector3 initialDirection = Vector3.down;
    [SerializeField] private float initialLaunchTime = 0.25f;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    private Vector3 direction;
    private Transform target; // Referencia al enemigo más cercano
    private bool initialLaunch = true;
    private SpriteRenderer spriteRenderer;
    private float rotationLimit = 0.85f;
    [SerializeField] float rotationSpeed = 360f;
    private Vector3 lastDirection;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = initialDirection;
        // Llama a la función para iniciar su movimiento
        StartCoroutine(startLaunching());
    }

    void Update()
    {
        if (target == null && !initialLaunch)
        {
            // Si no hay objetivo, intenta encontrar uno
            FindClosestEnemy();
            if (target == null) 
            {
                direction = lastDirection;
                return; // Si sigue sin haber objetivo, el misil no hace nada
            }

        }

        if (!initialLaunch)
        {
            // Dirigir el misil hacia el objetivo
            direction = (target.position - transform.position).normalized;
            float rotateAmount = Vector3.Cross(direction, transform.right).z;

            // transform.Rotate(0, 0, -rotateAmount * rotationSpeed * Time.deltaTime);
        }        
       
        transform.Translate(direction * speed * Time.deltaTime, Space.Self);
        lastDirection = direction;
    }

    void FindClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                target = enemy.transform;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el misil colisiona con el enemigo, destruye ambos
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // Destruye el enemigo
            Destroy(gameObject);           // Destruye el misil
        }
    }

    private IEnumerator startLaunching()
    {
        yield return new WaitForSeconds(initialLaunchTime);
        initialLaunch = false;
    }
}
