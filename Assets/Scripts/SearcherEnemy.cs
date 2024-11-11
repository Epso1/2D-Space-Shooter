using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearcherEnemy : Enemy
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private List<Sprite> directionSprites; // Lista de sprites para las diferentes direcciones
    private Vector2 direction;
    private Vector2 lastDirection = Vector2.left;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        InitializeEnemy();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {

        if (player != null)
        {
            // Calcular la dirección hacia el jugador
            direction = (player.position - transform.position).normalized;
            lastDirection = direction;

            // Mover el enemigo hacia la nueva posición
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
        else
        {
            // Si el jugador ha desaparecido, mueve al enemigo hacia la última dirección 
            rb.MovePosition(rb.position + lastDirection * speed * Time.fixedDeltaTime);
        }
        SetSpriteDirection(lastDirection);
    }

    void SetSpriteDirection(Vector3 direction)
    {
        // Determina el ángulo de la dirección en grados
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Asigna el sprite según el ángulo calculado
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
}
