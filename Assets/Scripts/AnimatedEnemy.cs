using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedEnemy : Enemy
{
    [SerializeField] private bool flipAnimationVertically = false;
    private Animator animator;
    private void Awake()
    {
        InitializeEnemy();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        // Si la posición inicial está en la mitad inferior de la pantalla, activa el Trigger que reproduce la animación invertida verticalmente
        if (flipAnimationVertically == true)
        {
            if (transform.position.y < 0)
            {
                animator.SetTrigger("FlipVertically");
            }
        }
    }

    private void ShootDiagonally()
    {        
        if (player != null)
        {
            Vector3 shotDirection = (player.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, bulletOrigin.position, Quaternion.identity);
            bullet.GetComponent<EnemyBullet>().direction = shotDirection;
        }

    }
}
