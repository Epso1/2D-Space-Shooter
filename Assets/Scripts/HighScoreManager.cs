using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HighScoreManager : Object
{
    public static string HighScoresKey = "HighScores";

    public void SaveHighScores(List<ScoreRecord> entries)
    {
        // Ordenar la lista en orden descendente de puntuación y guardar solo los 6 mejores
        entries = entries.OrderByDescending(entry => entry.score).Take(6).ToList();

        // Serializar la lista a un formato JSON
        string serializedEntries = JsonUtility.ToJson(new ScoreRecordListWrapper { entries = entries });
        PlayerPrefs.SetString(HighScoresKey, serializedEntries);
        PlayerPrefs.Save();
    }

    public List<ScoreRecord> LoadHighScores()
    {
        List<ScoreRecord> entries = new List<ScoreRecord>();

        if (PlayerPrefs.HasKey(HighScoresKey))
        {
            string serializedEntries = PlayerPrefs.GetString(HighScoresKey);

            if (!string.IsNullOrEmpty(serializedEntries))
            {
                try
                {
                    ScoreRecordListWrapper wrapper = JsonUtility.FromJson<ScoreRecordListWrapper>(serializedEntries);
                    if (wrapper != null && wrapper.entries != null)
                    {
                        entries = wrapper.entries;
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Error parsing JSON: " + e.Message);
                }
            }
        }

        return entries;
    }

    public void AddScore(ScoreRecord newEntry)
    {
        // Cargar las puntuaciones actuales
        List<ScoreRecord> currentHighScores = LoadHighScores();

        // Añadir la nueva entrada
        currentHighScores.Add(newEntry);

        // Guardar la lista actualizada
        SaveHighScores(currentHighScores);
    }
}

// Clase contenedora para serializar/deserializar listas de entradas
[System.Serializable]
public class ScoreRecordListWrapper
{
    public List<ScoreRecord> entries;
}
