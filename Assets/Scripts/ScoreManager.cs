using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton para acceso global
    public int currentScore;
    public int maxScore;
    public List<HighScore> highScores = new List<HighScore>();

    private const int maxHighScores = 6;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetCurrentScore();
    }

    public void AddPoints(int points)
    {
        currentScore += points;
        if (currentScore > maxScore)
        {
            maxScore = currentScore;
        }
    }

    public void ResetCurrentScore()
    {
        currentScore = 0;
    }

    public bool CheckIfHighScore()
    {
        if (highScores.Count < maxHighScores || currentScore > highScores[highScores.Count - 1].score)
        {
            return true;
        }
        return false;
    }

    public void AddHighScore(string initials)
    {
        HighScore newScore = new HighScore(initials, currentScore);
        highScores.Add(newScore);
        highScores.Sort((a, b) => b.score.CompareTo(a.score));

        if (highScores.Count > maxHighScores)
        {
            highScores.RemoveAt(highScores.Count - 1);
        }

        SaveHighScores();
    }

    private void LoadHighScores()
    {
        highScores.Clear();
        for (int i = 0; i < maxHighScores; i++)
        {
            string initials = PlayerPrefs.GetString($"HS{i}_Initials", "");
            int score = PlayerPrefs.GetInt($"HS{i}_Score", 0);

            if (!string.IsNullOrEmpty(initials))
            {
                highScores.Add(new HighScore(initials, score));
            }
        }
    }

    private void SaveHighScores()
    {
        for (int i = 0; i < highScores.Count; i++)
        {
            PlayerPrefs.SetString($"HS{i}_Initials", highScores[i].initials);
            PlayerPrefs.SetInt($"HS{i}_Score", highScores[i].score);
        }
        PlayerPrefs.Save();
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
