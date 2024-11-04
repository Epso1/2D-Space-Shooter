using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private AudioClip explosionSFX;
    private AudioSource audioSourceFX1;

    private void Awake()
    {
        audioSourceFX1 = GameObject.FindGameObjectWithTag("AudioSourceFX1").GetComponent<AudioSource>();
    }
    void Start()
    {
        audioSourceFX1.clip = explosionSFX;
        audioSourceFX1.Play();
    }

 
    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
