using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScoreEntry : MonoBehaviour
{
    [SerializeField] private string topScoresSceneName = "TopScores";
    [SerializeField] private TMPro.TextMeshProUGUI casilla1;
    [SerializeField] private TMPro.TextMeshProUGUI casilla2;
    [SerializeField] private TMPro.TextMeshProUGUI casilla3;
    [SerializeField] private Button doneButton; // Referencia al botón "Done"
    [SerializeField] private GameObject selectionFrame;

    // Array de caracteres que incluye letras y símbolos
    private char[] availableCharacters =
    {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
        'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        '?', '!', '$', '&', '#', '+', '-'
    };

    private char[] initials = { 'A', 'A', 'A' }; // Iniciales por defecto
    private int currentIndex = 0; // Índice de la casilla seleccionada (0-3, donde 3 es el botón Done)
    private Coroutine blinkCoroutine; // Corrutina para controlar el parpadeo


    private Vector2 moveInput; // Vector para capturar el input de movimiento
    private float blinkingTime = 0.25f;  // Duración del parpadeo de la inicial seleccionada
    private float loadNextSceneTime = 1f; // Espera para cargar la siguiente escena

    private void Start()
    {
        selectionFrame.SetActive(false);
        StartBlinking();
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (context.performed)
        {
            if (Mathf.Abs(moveInput.x) > 0.5f) // Mover horizontalmente
            {
                EnsureVisibilityBeforeSwitch();
                currentIndex = (currentIndex + (moveInput.x > 0 ? 1 : -1) + 4) % 4; // Ajustar índice
                StartBlinking();
                UpdateDisplay();
            }

            if (currentIndex < 3 && Mathf.Abs(moveInput.y) > 0.5f) // Navegación vertical en las iniciales
            {
                initials[currentIndex] = GetNextCharacter(initials[currentIndex], moveInput.y > 0 ? 1 : -1);
                UpdateDisplay();
            }
        }
    }

    public void Done(InputAction.CallbackContext context)
    {
        if (context.performed && currentIndex == 3) // Botón Done seleccionado
        {
            SaveInitials();
        }
    }

    private char GetNextCharacter(char currentChar, int step)
    {
        int index = System.Array.IndexOf(availableCharacters, currentChar);
        if (index == -1)
        {
            index = 0; // Si el carácter actual no está en el array, reiniciar a 'A'
        }
        index = (index + step + availableCharacters.Length) % availableCharacters.Length;
        return availableCharacters[index];
    }

    private void EnsureVisibilityBeforeSwitch()
    {
        if (currentIndex < 3)
        {
            // Asegurarse de que la casilla actual esté visible antes de cambiar de casilla
            SetAlpha(GetSelectedText(), 1f);
        }
    }

    private void StartBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }

        if (currentIndex < 3) // Las iniciales parpadean
        {
            selectionFrame.SetActive(false);
            blinkCoroutine = StartCoroutine(BlinkSelected());
        }
        else
        {
            selectionFrame.SetActive(true);
        }
    }

    private IEnumerator BlinkSelected()
    {
        TMPro.TextMeshProUGUI selectedText = GetSelectedText();
        while (true)
        {
            SetAlpha(selectedText, 0f); // Hacer el texto completamente transparente
            yield return new WaitForSeconds(blinkingTime); // Esperar
            SetAlpha(selectedText, 1f); // Hacer el texto completamente visible
            yield return new WaitForSeconds(blinkingTime); // Esperar
        }
    }

    private void SetAlpha(TMPro.TextMeshProUGUI text, float alpha)
    {
        if (text != null)
        {
            Color color = text.color;
            color.a = alpha;
            text.color = color;
        }
    }

    private TMPro.TextMeshProUGUI GetSelectedText()
    {
        switch (currentIndex)
        {
            case 0: return casilla1;
            case 1: return casilla2;
            case 2: return casilla3;
            default: return null; // Ninguna casilla para el botón Done
        }
    }

    private void UpdateDisplay()
    {
        casilla1.text = initials[0].ToString();
        casilla2.text = initials[1].ToString();
        casilla3.text = initials[2].ToString();

        if (currentIndex == 3) // Indicar selección del botón Done
        {
            doneButton.Select();
        }
    }

    public void SaveInitials()
    {
        // Convertir las iniciales a un string
        string finalInitials = new string(initials);

        // Obtener la puntuación actual del DataManager
        int finalScore = DataManager.Instance.score;

        // Crear un nuevo HighScore con las iniciales y la puntuación
        HighScore newEntry = new HighScore(finalInitials, finalScore);

        // Guardar el nuevo HighScore en la lista de máximas puntuaciones
        DataManager.Instance.AddHighScore(newEntry);

        Debug.Log("Record entered: " + finalInitials + " " + finalScore);

        // Cargar la escena de tabla de puntuaciones
        StartCoroutine(LoadHighScoresScene());
    }

    private IEnumerator LoadHighScoresScene()
    {
        yield return new WaitForSeconds(loadNextSceneTime);
        SceneManager.LoadScene(topScoresSceneName);
    }
}
