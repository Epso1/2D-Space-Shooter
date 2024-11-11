using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    [SerializeField] private float reloadSceneTime = 3f;
    [SerializeField] private AudioClip bgMusic;
    [SerializeField] private float bgMusicWait = 0.5f;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI topScoreText;

    private int score;
    public AudioSource audioSourceMusic;
    private HighScoreManager highScoreManager;
    private List<int> highScores;

    private static GameController _instance;

    public static GameController Instance { get { return _instance; } }


    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject); // Evita que se destruya al recargar la escena
            SceneManager.sceneLoaded += OnSceneLoaded; // Suscribirse al evento de carga de escena
        }

        
    }

    void Start()
    {
        // Solo reinicializar si es necesario
        if (audioSourceMusic == null)
        {
            audioSourceMusic = GameObject.FindWithTag("AudioSourceMusic").GetComponent<AudioSource>();
        }
        if (scoreText == null)
        {
            scoreText = GameObject.FindWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        }
        if (topScoreText == null)
        {
            topScoreText = GameObject.FindWithTag("TopScoreText").GetComponent<TextMeshProUGUI>();
        }

        highScoreManager = new HighScoreManager();
        highScores = highScoreManager.LoadHighScores();

        //// Imprimir las puntuaciones cargadas
        //Debug.Log("Top 6 Puntuaciones:");
        //foreach (int score in highScores)
        //{
        //    Debug.Log(score);
        //}

        Debug.Log(score);
        // Reproducir música de fondo
        StartCoroutine(PlayMusic());
    }

    void Update()
    {
        UpdateHighScores(score);
        int topScore = highScores[0];
        scoreText.text = "SCORE: " + score;
        topScoreText.text = "TOP SCORE: " + topScore;
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

    public void ReloadScene()
    {
        StartCoroutine(ReloadSceneEnum());
    }

    private IEnumerator ReloadSceneEnum()
    {
        yield return new WaitForSeconds(reloadSceneTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddPointsToScore(int points)
    {
        score += points;
    }

    public void UpdateHighScores(int newScore)
    {
        highScores.Add(newScore);
        highScoreManager.SaveHighScores(highScores);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reinicializar los componentes necesarios al cargar una nueva escena
        audioSourceMusic = GameObject.FindWithTag("AudioSourceMusic").GetComponent<AudioSource>();
        scoreText = GameObject.FindWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        topScoreText = GameObject.FindWithTag("TopScoreText").GetComponent<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        // Evita posibles referencias circulares o errores al destruir el objeto
        if (_instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
