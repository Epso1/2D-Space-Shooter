using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton para acceso global
    public int score;
    public int maxScore;
    public List<HighScore> highScores = new List<HighScore>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetCurrentScore();
        InitializeHighScores();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMultiply))
        {
            PrintHighScores();
        }

        UpdateHighScores();
    }

    public void AddPoints(int points)
    {
        score += points;
        if (score > maxScore)
        {
            maxScore = score;
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
    }

    public void LoadHighScores()
    {
        if (PlayerPrefs.HasKey("HighScores"))
        {
            string json = PlayerPrefs.GetString("HighScores");
            HighScoreListWrapper wrapper = JsonUtility.FromJson<HighScoreListWrapper>(json);
            highScores = wrapper.highScores;
        }
        else
        {
            highScores = new List<HighScore>();
        }
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
