using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour
{
    [SerializeField] private float reloadSceneTime = 3f;
    [SerializeField] private AudioClip bgMusic;
    [SerializeField] private float bgMusicWait = 0f;
    [SerializeField] private string gameOverSceneName = "GameOver";
    [SerializeField] private string enterInitialsSceneName = "EnterInitials";
    [SerializeField] private int lifes = 2;

    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI topScoreText;
    private TextMeshProUGUI lifesText;  
    private int score;
    private AudioSource audioSourceMusic;
    [HideInInspector] public HighScoreManager highScoreManager;
    private List<ScoreRecord> highScores;

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
        // Instancia y carga las máximas puntuaciones
        highScoreManager = new HighScoreManager();
        highScores = highScoreManager.LoadHighScores();

        // Asegura que la lista nunca sea `null`
        if (highScores == null || highScores.Count == 0)
        {
            highScores = new List<ScoreRecord> { new ScoreRecord("N/A", 0) };
        }
    }

    void Update()
    {
        UpdateHighScores(score);

        // Obtener la máxima puntuación de la lista
        int topScore = highScores.Max(entry => entry.score);

        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + score;
        }
        if (topScoreText != null)
        {
            topScoreText.text = "TOP SCORE: " + topScore;
        } 
        if (lifesText != null)
        {
            lifesText.text = "LIFES: " + lifes;
        } 
        
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            ResetMaxScore();
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            ShowHighScores();
        }
    }

    private IEnumerator PlayMusic()
    {
        yield return new WaitForSecondsRealtime(bgMusicWait);
        audioSourceMusic.clip = bgMusic;
        audioSourceMusic.Play();
    }

    public void PlayerDies()
    {
        if (lifes > -1)
        {
            StartCoroutine(ReloadSceneEnum());
        }
        else
        {
            lifes = 0;
            StartCoroutine(GameOver());
        }
    }

    private IEnumerator ReloadSceneEnum()
    {
        yield return new WaitForSeconds(reloadSceneTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(reloadSceneTime);

        // Cargar la lista de puntuaciones altas
        highScores = highScoreManager.LoadHighScores();

        // Comprobar si la puntuación actual está entre las 6 mejores
        bool isHighScore = score > highScores.Last().score;

        if (isHighScore)
        {
            // Si es una puntuación alta, cargar la escena para introducir iniciales
            SceneManager.LoadScene(enterInitialsSceneName);
        }
        else
        {
            // Si no, cargar la escena de "Game Over"
            SceneManager.LoadScene(gameOverSceneName);
        }
    }

    public void AddPointsToScore(int points)
    {
        score += points;
    }

    public void UpdateHighScores(int newScore)
    {
        highScores.Add(new ScoreRecord("N/A", newScore)); // Agregar la nueva puntuación con un marcador de iniciales
        highScores = highScores.OrderByDescending(entry => entry.score).Take(6).ToList(); // Mantener solo los 6 mejores
        highScoreManager.SaveHighScores(highScores);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        audioSourceMusic = GameObject.FindWithTag("AudioSourceMusic").GetComponent<AudioSource>();

        if (GameObject.FindWithTag("ScoreText") == true)
        {
            scoreText = GameObject.FindWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        }
        if (GameObject.FindWithTag("TopScoreText")) {
            topScoreText = GameObject.FindWithTag("TopScoreText").GetComponent<TextMeshProUGUI>();
        }
        if (GameObject.FindWithTag("LifesText"))
        {
            lifesText = GameObject.FindWithTag("LifesText").GetComponent<TextMeshProUGUI>();
        }

        StartCoroutine(PlayMusic());
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public int GetCurrentScore()
    {
        return score;
    }

    public void LoseOneLife()
    {
        lifes--;
    }

    public void ResetMaxScore()
    {
        highScoreManager.ResetHighScores();
        Debug.Log("Max score has been reset.");
    }

    public void ShowHighScores()
    {
        highScoreManager.PrintHighScores();
    }
}
