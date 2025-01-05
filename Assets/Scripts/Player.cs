using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Player : MonoBehaviour
{
    // Eventos que la nave fantasma escuchará
    public static event Action OnShoot;
    public static event Action OnShootMissile;
    public static event Action OnPlayerDies;

    [SerializeField] public float speed = 2f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private Transform missileOrigin;
    [SerializeField] private AudioClip shotSFX;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float missileCooldownTime = 2f;
    [SerializeField] GameObject multiplePrefab;

    private Vector2 moveInput;
    private PlayerInput playerInput;
    private Rigidbody2D rb2D;
    private Animator anim;

    private Vector2 screenBounds;  // límites de la pantalla
    private float playerWidth;
    private float playerHeight;
    public bool missileEnabled = false;
    public bool multipleEnabled = false;

    private bool canShootMissile = true; // Bandera para controlar el tiempo de recarga

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Obtener el tamaño del sprite del jugador
        playerWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        playerHeight = GetComponent<SpriteRenderer>().bounds.extents.y;

        // Definir los límites de la pantalla en unidades del mundo
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Start()
    {
        // Inicializa el estado de los powerUps
        PowerUpManager.Instance.LoadPowerUpData();

        if (multipleEnabled)
        {
            if (GameObject.FindWithTag("Multiple") == null)
            {
                Instantiate(multiplePrefab, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
            }
        }
    }

    void Update()
    {
        // Almacena el input de movimiento en la variable moveInput
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        // Animaciones
        if (moveInput.y > 0)
        {
            anim.Play("Player_Move_Up");
        }
        else if (moveInput.y <= 0)
        {
            anim.Play("Player_Idle");
        }
    }

    private void FixedUpdate()
    {
        if (moveInput != Vector2.zero)
        {
            Vector2 newPosition = rb2D.position + moveInput * speed * Time.fixedDeltaTime;
            newPosition = ClampPositionToScreen(newPosition);  // Limitar posición
            rb2D.MovePosition(newPosition);
        }
    }

    private Vector2 ClampPositionToScreen(Vector2 position)
    {
        float clampedX = Mathf.Clamp(position.x, screenBounds.x * -1 + playerWidth, screenBounds.x - playerWidth);
        float clampedY = Mathf.Clamp(position.y, screenBounds.y * -1 + playerHeight, screenBounds.y - playerHeight);
        return new Vector2(clampedX, clampedY);
    }

    public void Shoot(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            Instantiate(bulletPrefab, bulletOrigin.position, Quaternion.identity);
            // Disparar evento
            OnShoot?.Invoke();
        }
    }

    public void shootMissile(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && missileEnabled && canShootMissile)
        {
            Instantiate(missilePrefab, missileOrigin.position, Quaternion.identity);
            // Disparar evento
            OnShootMissile?.Invoke();

            // Deshabilitar disparar misiles por 3 segundos
            StartCoroutine(MissileCooldown());
        }
    }

    private IEnumerator MissileCooldown()
    {
        canShootMissile = false;
        yield return new WaitForSeconds(missileCooldownTime); // Tiempo de recarga de 3 segundos
        canShootMissile = true;
    }

    private void PlayerDies()
    {
        // Disparar evento
        OnPlayerDies?.Invoke();
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        PowerUpManager.Instance.ResetPowerUpData();
        DataManager.Instance.LoseOneLife();        
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Obstacle") || collision.CompareTag("EnemyBullet"))
        {
            PlayerDies();
        }
    }
}
