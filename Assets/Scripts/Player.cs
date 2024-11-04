using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveVelocity = 1f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private AudioClip shotSFX;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameManager gameManager;

    private Vector2 moveInput;
    private PlayerInput playerInput;
    private Rigidbody2D rb2D;
    private Animator anim;

    private Vector2 screenBounds;  // l�mites de la pantalla
    private float playerWidth;
    private float playerHeight;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Obtener el tama�o del sprite del jugador
        playerWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        playerHeight = GetComponent<SpriteRenderer>().bounds.extents.y;

        // Definir los l�mites de la pantalla en unidades del mundo
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
            newPosition = ClampPositionToScreen(newPosition);  // Limitar posici�n
            rb2D.MovePosition(newPosition);
        }
    }

    // M�todo para limitar la posici�n del jugador dentro de los l�mites de la pantalla
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
            Shoot();
        }
    }

    private void PlayerDies()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        gameManager.ReloadScene();
        Destroy(gameObject);
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, bulletOrigin.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Obstacle"))
        {
            PlayerDies();
        }
        
    }
}
