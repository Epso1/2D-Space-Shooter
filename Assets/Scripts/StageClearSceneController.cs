using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageClearSceneController : MonoBehaviour
{
    [SerializeField] private AudioSource UIAudioSource;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string enterInitialsSceneName = "InitialsEntry";
    [SerializeField] private string gameOverSceneName = "GameOver";
    [SerializeField] private Button[] buttons; // Lista de botones en el menú
    [SerializeField] private AudioClip changeSelectionSFX;
    [SerializeField] private string nextSceneName = "Scene02";
    [SerializeField] private TextMeshProUGUI scoreText;

    private Color color0, color1, color2;
    private int selectedIndex = 0; // Índice del botón seleccionado

    private void Awake()
    {
        color2 = new Color(.784f, .784f, .784f);
        color1 = Color.white;
        color0 = Color.white;
        color0.a = 0f;
    }
    private void Start()
    {
        scoreText.text = DataManager.Instance.score.ToString();
    }
    private void Update()
    {

    }

    public void Move(InputAction.CallbackContext context)
    {
       

    }
   
    public void Submit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            buttons[selectedIndex].GetComponent<Image>().color = color2;
            buttons[selectedIndex].onClick.Invoke();
        }
    }

    public void ToNextStage()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void ChechHighScore()
    {
        // Comprobar si la puntuación actual está entre las 6 mejores
        bool isHighScore = DataManager.Instance.score > DataManager.Instance.highScores[DataManager.Instance.highScores.Count - 1].score;

        if (isHighScore)
        {           
            // Si es una puntuación alta, cargar la escena para introducir iniciales
            PowerUpManager.Instance.isTopScore = true;
            SceneManager.LoadScene(enterInitialsSceneName);
        }
        else
        {
            // Si no, ir a la escena de Game Over
            SceneManager.LoadScene(gameOverSceneName);
        }
    }


}
