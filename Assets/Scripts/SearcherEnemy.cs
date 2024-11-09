using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearcherEnemy : Enemy
{
    [SerializeField] private float speed = 2f;
    private Vector2 direction;
    private Vector2 lastDirection = Vector2.left;

    private void Awake()
    {
        InitializeEnemy();
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
    }
}
