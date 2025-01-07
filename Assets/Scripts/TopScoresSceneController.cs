using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TopScoresSceneController : MonoBehaviour
{

    [SerializeField] private AudioSource UIAudioSource;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string gameOverSceneName = "GameOver";
    [SerializeField] private Button[] buttons; // Lista de botones en el menú
    [SerializeField] private AudioClip changeSelectionSFX;

    private Color color0, color1, color2;
    private int selectedIndex = 0; // Índice del botón seleccionado

    private void Awake()
    {
        color2 = new Color(.784f, .784f, .784f);
        color1 = Color.white;
        color0 = Color.white;
        color0.a = 0f;
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

    public void ToNextScene()
    {
        if (PowerUpManager.Instance.isTopScore)
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
        else
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}
