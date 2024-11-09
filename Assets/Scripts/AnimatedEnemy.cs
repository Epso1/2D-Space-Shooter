using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedEnemy : Enemy
{
    [SerializeField] private bool flipAnimationHorizontally = false;
    private Animator animator;
    private void Awake()
    {
        InitializeEnemy();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        // Si la posición inicial en el eje y es inferior a 0, activa el Trigger que reproduce la animación invertida horizontalmente
        if (flipAnimationHorizontally == true)
        {
            if (transform.position.y < 0)
            {
                animator.SetTrigger("FlipHorizontally");
            }
        }
    }
}
