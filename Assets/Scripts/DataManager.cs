using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    private static DataManager instance; // Singleton para acceso global

    public static DataManager Instance { get { return instance; } }
    [HideInInspector] public int score = 0;
    private int topScore;
    public List<HighScore> highScores = new List<HighScore>();
    [HideInInspector] public int lifes = 3;
    private GameController gameController;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Suscribirse al evento de carga de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            PrintHighScores();
        }

        if (score > topScore)
        {
            topScore = score;
            gameController.UpdateTopScore(topScore);
        }
    }

    public void AddPoints(int points)
    {
        score += points;
        gameController.UpdateScore(score);
        if (score > topScore)
        {
            topScore = score;
            gameController.UpdateTopScore(topScore);
        }
    }

    public void ResetCurrentScore()
    {
        score = 0;
    }

    public void InitializeHighScores()
    {
        highScores.Add(new HighScore("UNO", 1500));
        highScores.Add(new HighScore("SEI", 1000));
        highScores.Add(new HighScore("DOS", 1400));
        highScores.Add(new HighScore("CUA", 1200));
        highScores.Add(new HighScore("CIN", 1100));
        highScores.Add(new HighScore("TRE", 1300));

        highScores.Sort((a, b) => b.score.CompareTo(a.score));

        string json = JsonUtility.ToJson(new HighScoreListWrapper(highScores));
        PlayerPrefs.SetString("HighScores", json);
        PlayerPrefs.Save();
        LoadHighScores();
    }

    public void LoadHighScores()
    {
        if (PlayerPrefs.HasKey("HighScores"))
        {
            string json = PlayerPrefs.GetString("HighScores");
            HighScoreListWrapper wrapper = JsonUtility.FromJson<HighScoreListWrapper>(json);
            highScores = wrapper.highScores;
            highScores.Sort((a, b) => b.score.CompareTo(a.score));
            
        }
        else
        {
            InitializeHighScores();
        }
        topScore = highScores[0].score;
    }
    public void PrintHighScores()
    {
        foreach (HighScore highScore in highScores)
        {
            Debug.Log($"Initials: {highScore.initials}, Score: {highScore.score}");
        }
    }
    public bool IsHighScore(int currentScore)
    {
        if (currentScore > highScores[highScores.Count - 1].score)
        {
            return true;
        }
        return false;
    }

    public void UpdateHighScores()
    {
        // Verifica si el puntaje actual califica para entrar en la lista
        if (IsHighScore(score))
        {
            highScores.Sort((a, b) => b.score.CompareTo(a.score));

            for (int i = (highScores.Count - 1); i >= 0; i--)
            {
                if (score > highScores[i].score)
                {
                    highScores[i] = new HighScore(highScores[i].initials, highScores[i].score);
                    highScores.Add(new HighScore("***", score));
                    highScores.Add(highScores[i]);

                    highScores.Sort((a, b) => b.score.CompareTo(a.score));

                    if (highScores.Count > 5) // Asume que quieres solo los 5 mejores puntajes
                    {
                        for (int j = 0; j < (highScores.Count -5); j++)
                        {
                            highScores.RemoveAt(highScores.Count - 1); // Elimina el más bajo
                        }                        
                    }
                }
            }
            // Guarda los puntajes actualizados en PlayerPrefs
            string json = JsonUtility.ToJson(new HighScoreListWrapper(highScores));
            PlayerPrefs.SetString("HighScores", json);
            PlayerPrefs.Save();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadHighScores();

        if (GameObject.FindGameObjectWithTag("GameController"))
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            gameController.UpdateTopScore(topScore);
            gameController.UpdateScore(score);
            gameController.UpdateLifes(lifes);
        }
        
       
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public void LoseOneLife()
    {
        lifes--;
        gameController.UpdateLifes(lifes);
        gameController.PlayerDies();
    }

    public void AddHighScore(HighScore newHighScore)
    {

    }
}

[System.Serializable]
public class HighScoreListWrapper
{
    public List<HighScore> highScores;

    public HighScoreListWrapper(List<HighScore> scores)
    {
        highScores = scores;
    }
}

[System.Serializable]
public class HighScore
{
    public string initials;
    public int score;

    public HighScore(string initials, int score)
    {
        this.initials = initials;
        this.score = score;
    }
}
