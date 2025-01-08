using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private AudioClip explosionSFX;
    private AudioSource audioSourceFX1;
    [SerializeField] private bool movingExplosion = false;
    [SerializeField] private float bgVelocity;
    private Vector3 direction = -Vector3.right;

    private void Awake()
    {
        audioSourceFX1 = GameObject.FindGameObjectWithTag("AudioSourceFX1").GetComponent<AudioSource>();
    }
    void Start()
    {
        audioSourceFX1.clip = explosionSFX;
        audioSourceFX1.Play();
    }
    private void Update()
    {
        if (movingExplosion)
        {
            transform.position += direction * bgVelocity * Time.deltaTime;
        }
    }


    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
