using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SocialPlatforms.Impl;

public class GameController : MonoBehaviour
{
    [SerializeField] private float reloadSceneTime = 3f;
    [SerializeField] private AudioClip bgMusic;
    [SerializeField] private float bgMusicWait = 0f;
    [SerializeField] private string gameOverSceneName = "GameOver";
    [SerializeField] private string enterInitialsSceneName = "EnterInitials";

    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI topScoreText;
    private TextMeshProUGUI lifesText;  
    private AudioSource audioSourceMusic;

    
    void Awake()
    {
        audioSourceMusic = GameObject.FindWithTag("AudioSourceMusic").GetComponent<AudioSource>();
        scoreText = GameObject.FindWithTag("ScoreText").GetComponent<TextMeshProUGUI>();         
        topScoreText = GameObject.FindWithTag("TopScoreText").GetComponent<TextMeshProUGUI>();      
        lifesText = GameObject.FindWithTag("LifesText").GetComponent<TextMeshProUGUI>();        

        StartCoroutine(PlayMusic());
    }

 
    private IEnumerator PlayMusic()
    {
        yield return new WaitForSecondsRealtime(bgMusicWait);
        audioSourceMusic.clip = bgMusic;
        audioSourceMusic.Play();
    }

    public void PlayerDies()
    {
        if (DataManager.Instance.lifes > -1)
        {
            StartCoroutine(ReloadSceneEnum());
        }
        else
        {
            DataManager.Instance.lifes = 0;
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

        // Comprobar si la puntuación actual está entre las 6 mejores
        bool isHighScore = DataManager.Instance.score > DataManager.Instance.highScores[DataManager.Instance.highScores.Count - 1].score;

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


    public void UpdateScore(int score)
    {
        scoreText.text = "SCORE: " + score;
    }

    public void UpdateTopScore(int topScore)
    {
        topScoreText.text = "TOP SCORE: " + topScore;
    }

    public void UpdateLifes(int lifes)
    {
        int currentLifes = lifes;
        if (lifes < 0)
        {
            currentLifes = 0;
        }
      
        lifesText.text = "LIFES: " + currentLifes;
    }
}
