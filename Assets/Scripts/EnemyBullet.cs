using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [HideInInspector] public Vector2 direction;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private AudioClip bulletSFX;
    private Rigidbody2D rb2D;
    private AudioSource audioSourceFX3;
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        audioSourceFX3 = GameObject.FindWithTag("AudioSourceFX3").GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSourceFX3.clip = bulletSFX;
        audioSourceFX3.Play();
    }


    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + direction * bulletSpeed * Time.fixedDeltaTime);
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
