using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiple : MonoBehaviour
{
    private Transform player;  // Referencia al jugador
    public float delayTime = 0.2f; // Tiempo de retraso en segundos

    private List<Vector3> playerPositions; // Lista para almacenar las posiciones del jugador
    private int delayFrames; // Número de frames de retraso basado en delayTime
    private Vector3 lastPlayerPosition; // Última posición conocida del jugador


    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private Transform missileOrigin;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerPositions = new List<Vector3>();
        delayFrames = Mathf.CeilToInt(delayTime / Time.fixedDeltaTime);
        lastPlayerPosition = player.position;
    }

    void FixedUpdate()
    {
        // Comprobar si el jugador se ha movido
        if (player.position != lastPlayerPosition)
        {
            // Añadir la posición actual del jugador a la lista si se movió
            playerPositions.Insert(0, player.position);
            lastPlayerPosition = player.position;

            // Asegurarse de que la lista no crezca demasiado
            if (playerPositions.Count > delayFrames)
            {
                // Mover la nave fantasma a la posición retrasada del jugador
                transform.position = playerPositions[delayFrames];

                // Eliminar la posición más antigua
                playerPositions.RemoveAt(playerPositions.Count - 1);
            }
        }
    }

    void OnEnable()
    {
        // Subscribirse a los eventos del jugador
        Player.OnShoot += HandleShoot;
        Player.OnShootMissile += HandleShootMissile;
    }

    void OnDisable()
    {
        // Desubscribirse de los eventos cuando se desactiva el objeto
        Player.OnShoot -= HandleShoot;
        Player.OnShootMissile -= HandleShootMissile;
    }

    private void HandleShoot()
    {
        // Disparar un proyectil
        Instantiate(bulletPrefab, bulletOrigin.position, Quaternion.identity);
    }

    private void HandleShootMissile()
    {
        // Lanzar un misil
        Instantiate(missilePrefab, missileOrigin.position, Quaternion.identity);
    }
}
