using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonEnemy : MonoBehaviour
{
    private Animator animator;
    private Transform player;
    [SerializeField] private float playerDetectionRange = 5f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private string openAnimationName;
    [SerializeField] private string closeAnimationName;
    [SerializeField] private int shotsQuantity = 5;
    private bool isPlayerClose = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (!isPlayerClose)
        {
            CheckPlayerXDistance();
        }
        
    }

    void CheckPlayerXDistance()
    {
        if (player != null)
        {
            // Calcular la distancia al jugador
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            Debug.Log(distanceToPlayer);

            // Si el jugador est� dentro del rango y el tiempo de recarga ha pasado
            if (distanceToPlayer <= playerDetectionRange)
            {
                StartCoroutine(Shoot());
            }
        }
    }

    IEnumerator Shoot()
    {     
        isPlayerClose = true;
        
        yield return new WaitForSeconds(0.5f);

        
        if (player != null)
        {
            animator.Play(openAnimationName);
            yield return new WaitForSeconds(0.3f);
            Vector3 shotDirection = (player.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, bulletOrigin.position, Quaternion.identity);
            bullet.GetComponent<EnemyBullet>().direction = shotDirection;                
            animator.Play(closeAnimationName);
            yield return new WaitForSeconds(0.3f);
        }       
        
        animator.Play(closeAnimationName);
        isPlayerClose = false;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
