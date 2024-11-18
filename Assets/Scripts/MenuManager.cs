using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource UIAudioSource;
    [SerializeField] private AudioClip startGameSFX;
    [SerializeField] private string scene01Name = "Scene01";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        StartCoroutine(StarGameEnum());
    }

    private IEnumerator StarGameEnum()
    {
        UIAudioSource.clip = startGameSFX;
        UIAudioSource.Play();
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(scene01Name);
    }
}
