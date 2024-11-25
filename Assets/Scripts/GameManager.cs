using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
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

    private void Start()
    {
        audioSourceMusic = GameObject.FindWithTag("AudioSourceMusic").GetComponent<AudioSource>();

        if (GameObject.FindWithTag("ScoreText") == true)
        {
            scoreText = GameObject.FindWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        }
        if (GameObject.FindWithTag("TopScoreText"))
        {
            topScoreText = GameObject.FindWithTag("TopScoreText").GetComponent<TextMeshProUGUI>();
        }
        if (GameObject.FindWithTag("LifesText"))
        {
            lifesText = GameObject.FindWithTag("LifesText").GetComponent<TextMeshProUGUI>();
        }

        StartCoroutine(PlayMusic());
    }
    public void EndGame()
    {
        if (ScoreManager.Instance.CheckIfHighScore())
        {
            SceneManager.LoadScene(enterInitialsSceneName);
        }
        else
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
    }

    public void RestartGame()
    {
        ScoreManager.Instance.ResetCurrentScore();
        SceneManager.LoadScene("GameScene");
    }

    private IEnumerator PlayMusic()
    {
        yield return new WaitForSecondsRealtime(bgMusicWait);
        audioSourceMusic.clip = bgMusic;
        audioSourceMusic.Play();
    }
}
