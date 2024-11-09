using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float reloadSceneTime = 3f;
    [SerializeField] private AudioClip bgMusic;
    [SerializeField] private float bgMusicWait = 0.5f;

    private AudioSource audioSourceMusic;

    private void Awake()
    {
        // Inicializa el AudioSource de la música
        audioSourceMusic = GameObject.FindGameObjectWithTag("AudioSourceMusic").GetComponent<AudioSource>();
    }
    void Start()
    {
       StartCoroutine(PlayMusic());
    }

    void Update()
    {
        
    }
    private IEnumerator PlayMusic()
    {
        // Espera inicial
        yield return new WaitForSecondsRealtime(bgMusicWait);
        // Carga el clip de la música en el AudioSource
        audioSourceMusic.clip = bgMusic;
        // Reproduce la música de fondo
        audioSourceMusic.Play();
    }
    // Función para llamar a la corrutina para recargar escena actual
    public void ReloadScene()
    {
        StartCoroutine(ReloadSceneEnum());
    }
    // Corrutina para recargar escena actual
    private IEnumerator ReloadSceneEnum()
    {
        yield return new WaitForSeconds(reloadSceneTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
