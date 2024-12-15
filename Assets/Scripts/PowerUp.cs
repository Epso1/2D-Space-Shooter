using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerUp : MonoBehaviour
{
    public UnityEvent powerUpEffect;
    private float speed = 2f;
    [SerializeField] Vector2 initialDirection = Vector2.left;
    [SerializeField] float maxPlayerSpeed = 6.5f;
    [SerializeField] AudioClip speedUpVoice;
    [SerializeField] AudioClip multipleVoice;
    [SerializeField] AudioClip missileVoice;
    [SerializeField] GameObject multiplePrefab;
    private int pointsWhenCollected = 100;
    private Player player;
    private AudioSource audioSourceVoice;
    private Rigidbody2D rb2D;

    void Start()
    {
        if (GameObject.FindWithTag("Player"))
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }        
        audioSourceVoice = GameObject.FindWithTag("AudioSourceVoice").GetComponent<AudioSource>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + initialDirection * speed * Time.fixedDeltaTime);
    }

    public void EnableMissile()
    {
        
        if (player != null && player.missileEnabled == false)
        {
            PlayMissileVoice();
            player.missileEnabled = true;
        }        
    }

    public void PlayMissileVoice()
    {
        audioSourceVoice.clip = missileVoice;
        audioSourceVoice.Play();
    }

    public void SpeedUp()
    {
        if (player.speed < maxPlayerSpeed)
        {
            PlaySpeedUpVoice();
            player.speed *= 1.25f;
        }
    }

    public void PlaySpeedUpVoice()
    {
        audioSourceVoice.clip = speedUpVoice;
        audioSourceVoice.Play();
    }

    public void Multiple()
    {
        if (GameObject.FindWithTag("Multiple") == null)
        {
            PlayMultipleVoice();
            Instantiate(multiplePrefab, player.transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
        }     
    }
    public void PlayMultipleVoice()
    {
        audioSourceVoice.clip = multipleVoice;
        audioSourceVoice.Play();
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DataManager.Instance.AddPoints(pointsWhenCollected);
            powerUpEffect.Invoke();
            Destroy(gameObject);
        }
    }
}
