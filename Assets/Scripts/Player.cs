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

    [SerializeField] private float moveVelocity = 1f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private Transform missileOrigin;
    [SerializeField] private AudioClip shotSFX;
    [SerializeField] private GameObject explosionPrefab;

    private Vector2 moveInput;
    private PlayerInput playerInput;
    private Rigidbody2D rb2D;
    private Animator anim;

    private Vector2 screenBounds;  // límites de la pantalla
    private float playerWidth;
    private float playerHeight;

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

    void Update()
    {
        // Almacenamos el input de movimiento en la variable moveInput
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
            Vector2 newPosition = rb2D.position + moveInput * moveVelocity * Time.fixedDeltaTime;
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
        if (callbackContext.performed)
        {
            Instantiate(missilePrefab, missileOrigin.position, Quaternion.identity);
            // Disparar evento
            OnShootMissile?.Invoke();
        }
    }

    private void PlayerDies()
    {
        // Disparar evento
        OnPlayerDies?.Invoke();
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
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
