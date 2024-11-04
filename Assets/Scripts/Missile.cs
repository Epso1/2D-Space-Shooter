using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;                       // Velocidad del misil
    [SerializeField] private float initialWait = 1f;                 // Tiempo inicial antes de buscar al enemigo
    [SerializeField] private Vector2 initialDirection = Vector2.up;  // Direcci�n inicial de vuelo
    [SerializeField] private float obstacleDetectionDistance = 1.5f; // Distancia de detecci�n de obst�culos
    [SerializeField] private float avoidanceAngle = 30f;             // �ngulo para esquivar obst�culos
    [SerializeField] private float avoidanceTime = 1f;               // Tiempo que mantendr� la direcci�n de evasi�n
    [SerializeField] private LayerMask obstacleLayerMask;            // LayerMask para filtrar colisiones   
    [SerializeField] private List<Sprite> directionSprites;          // Lista de sprites para las diferentes direcciones

    private Transform target;                      // Referencia al enemigo m�s cercano
    private SpriteRenderer spriteRenderer;         // SpriteRenderer del misil
    private bool isChasingEnemy = false;           // Flag para saber cu�ndo perseguir al enemigo
    private float waitTime;                        // Contador de espera
    private float avoidanceTimer = 0;             // Temporizador para mantener direcci�n de evasi�n
    private Vector3? lastKnownPosition = null;     // �ltima posici�n conocida del enemigo
    private Vector3 currentDirection;               // Direcci�n actual del misil
    private Vector3 targetDirection;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        waitTime = initialWait;                    // Inicializa el contador de espera
        currentDirection = initialDirection;       // Establece la direcci�n inicial
        SetSpriteDirection(currentDirection);
    }

    void Update()
    {
        if (waitTime > 0)
        {
            // Vuelo en la direcci�n inicial durante el tiempo de espera a una velocidad reducida (5 veces menor)
            transform.Translate(initialDirection.normalized * speed / 5 * Time.deltaTime, Space.World);
            waitTime -= Time.deltaTime;
        }
        else
        {
            // Si ya ha pasado el tiempo de espera, comienza la persecuci�n si a�n no lo ha hecho
            if (!isChasingEnemy)
            {
                FindClosestEnemy();
                if (target != null)
                {
                    isChasingEnemy = true;
                }
                else { waitTime = 5f; }
                
            }

            if (target != null)
            {
                lastKnownPosition = target.position;
                targetDirection = (target.position - transform.position).normalized;
                if (lastKnownPosition != null)
                {
                    targetDirection = ((Vector3)lastKnownPosition - transform.position).normalized;
                }

            }
            

            // Si el temporizador de evasi�n est� activo, mantenemos la direcci�n de evasi�n actual
            if (avoidanceTimer > 0)
            {
                avoidanceTimer -= Time.deltaTime;
            }
            else
            {
                // Comprobar si hay obst�culos en la direcci�n de movimiento
                if (DetectObstacle(targetDirection))
                {
                    Debug.Log("Obstacle detected");
                    // Si hay un obst�culo, calcula una direcci�n alternativa y activa el temporizador de evasi�n
                    currentDirection = AvoidObstacle(targetDirection);
                    avoidanceTimer = avoidanceTime;  // Reinicia el temporizador de evasi�n
                }
                else
                {
                    // Si no hay obst�culos, usa la direcci�n hacia el objetivo
                    currentDirection = targetDirection;
                }
            }

            // Mover el misil en la direcci�n calculada y actualizar el sprite
            SetSpriteDirection(currentDirection);
            transform.Translate(currentDirection * speed * Time.deltaTime);
        }
    }

    bool DetectObstacle(Vector3 direction)
    {
        // Realiza un RayCast hacia adelante usando el LayerMask para ignorar el layer del misil
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, obstacleDetectionDistance, obstacleLayerMask);

        return hit.collider != null;
    }

    Vector3 AvoidObstacle(Vector3 originalDirection)
    {
        //Debug.Log("Avoiding obstacle in direction: " + originalDirection);
        // Gira el vector original a la izquierda o derecha en un �ngulo definido para evitar el obst�culo
        Vector3 avoidanceDirection1 = Quaternion.Euler(0, 0, -avoidanceAngle) * originalDirection;
        Vector3 avoidanceDirection2 = Quaternion.Euler(0, 0, avoidanceAngle) * originalDirection;

        // Comprueba si alguna de las direcciones alternativas est� libre de obst�culos
        if (!DetectObstacle(avoidanceDirection1))
        {
            //Debug.Log("Changing direction to avoiding1 : " + avoidanceDirection1);
            return avoidanceDirection1;
        }
        else if (!DetectObstacle(avoidanceDirection2))
        {
            //Debug.Log("Changing direction to avoiding2: " + avoidanceDirection2);
            return avoidanceDirection2;
        }

        // Si ambas direcciones est�n bloqueadas, contin�a en la direcci�n original (forzar� colisi�n)
        return originalDirection;
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

    void SetSpriteDirection(Vector3 direction)
    {
        // Determina el �ngulo de la direcci�n en grados
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Asigna el sprite seg�n el �ngulo calculado
        if (angle >= -22.5f && angle < 22.5f)
            spriteRenderer.sprite = directionSprites[0]; // Derecha
        else if (angle >= 22.5f && angle < 67.5f)
            spriteRenderer.sprite = directionSprites[7]; // Diagonal arriba-derecha
        else if (angle >= 67.5f && angle < 112.5f)
            spriteRenderer.sprite = directionSprites[6]; // Arriba
        else if (angle >= 112.5f && angle < 157.5f)
            spriteRenderer.sprite = directionSprites[5]; // Diagonal izquierda-arriba
        else if (angle >= -67.5f && angle < -22.5f)
            spriteRenderer.sprite = directionSprites[1]; // Diagonal derecha-abajo
        else if (angle >= -112.5f && angle < -67.5f)
            spriteRenderer.sprite = directionSprites[2]; // Abajo
        else if (angle >= -157.5f && angle < -112.5f)
            spriteRenderer.sprite = directionSprites[3]; // Diagonal abajo-izquierda
        else
            spriteRenderer.sprite = directionSprites[4]; // Izquierda
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject); // Destruye el misil
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject); // Destruye el misil cuando sale de la pantalla
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 centerDirection = currentDirection * obstacleDetectionDistance;
        Gizmos.DrawRay(transform.position, centerDirection);
    }
}
