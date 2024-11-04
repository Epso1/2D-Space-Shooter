using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 50f;
    [SerializeField] private AudioClip bulletSFX;
    private Vector2 direction = Vector2.right;
    private Rigidbody2D rb2D;
    private AudioSource audioSourceFX2;
    private bool canMove = false;

    private void Awake()
    {
        audioSourceFX2 = GameObject.FindWithTag("AudioSourceFX2").GetComponent<AudioSource>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        audioSourceFX2.clip = bulletSFX;
        audioSourceFX2.Play();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rb2D.MovePosition(rb2D.position + direction * bulletSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void SetCanMoveTrue()
    {
        canMove = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
