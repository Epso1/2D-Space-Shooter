using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : Object
{
    private const string HighScoresKey = "HighScores";

    public void SaveHighScores(List<int> scores)
    {
        // Ordenar la lista en orden descendente y guardar solo los 6 mejores
        scores.Sort((a, b) => b.CompareTo(a));
        if (scores.Count > 6)
        {
            scores = scores.GetRange(0, 6);
        }

        // Serializar la lista a un string separado por comas
        string serializedScores = string.Join(",", scores);
        PlayerPrefs.SetString(HighScoresKey, serializedScores);
        PlayerPrefs.Save();
    }

    public List<int> LoadHighScores()
    {
        List<int> scores = new List<int>();

        if (PlayerPrefs.HasKey(HighScoresKey))
        {
            // Deserializar la lista desde el string almacenado
            string serializedScores = PlayerPrefs.GetString(HighScoresKey);
            string[] scoreStrings = serializedScores.Split(',');

            foreach (string score in scoreStrings)
            {
                if (int.TryParse(score, out int parsedScore))
                {
                    scores.Add(parsedScore);
                }
            }
        }

        return scores;
    }
}

