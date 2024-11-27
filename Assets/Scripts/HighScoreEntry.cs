using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class HighScoreEntry : MonoBehaviour
{
    public TMPro.TextMeshProUGUI casilla1;
    public TMPro.TextMeshProUGUI casilla2;
    public TMPro.TextMeshProUGUI casilla3;

    // Array de caracteres que incluye letras y símbolos
    private char[] availableCharacters =
    {
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
        'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
        '?', '!', '$', '&', '+', '-', '@', '♥'
    };

    private char[] initials = { 'A', 'A', 'A' }; // Iniciales por defecto
    private int currentIndex = 0; // Índice de la casilla seleccionada
    private Coroutine blinkCoroutine; // Corrutina para controlar el parpadeo

    private PlayerInput playerInput; // Referencia al sistema de input
    private Vector2 moveInput; // Vector para capturar el input de movimiento

    private float blinkingTime = 0.25f;  //Duración del parpadeo de la inicial seleccionada


    private void Awake()
    {
        // Inicializar PlayerInput y asignar el callback de movimiento
        playerInput = GetComponent<PlayerInput>();

    }

    private void Start()
    {
        StartBlinking();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (context.performed)
        {
            if (moveInput.x > 0.5f) // Mover a la derecha
            {
                EnsureVisibilityBeforeSwitch();
                currentIndex = (currentIndex + 1) % 3;
                StartBlinking();
                UpdateDisplay();
            }
            else if (moveInput.x < -0.5f) // Mover a la izquierda
            {
                EnsureVisibilityBeforeSwitch();
                currentIndex = (currentIndex - 1 + 3) % 3;
                StartBlinking();
                UpdateDisplay();
            }

            if (moveInput.y > 0.5f) // Subir la letra/símbolo
            {
                initials[currentIndex] = GetNextCharacter(initials[currentIndex], 1);
                UpdateDisplay();
            }
            else if (moveInput.y < -0.5f) // Bajar la letra/símbolo
            {
                initials[currentIndex] = GetNextCharacter(initials[currentIndex], -1);
                UpdateDisplay();
            }
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
        // Asegurarse de que la casilla actual esté visible antes de cambiar de casilla
        GetSelectedText().enabled = true;
    }

    private void StartBlinking()
    {
        // Detener cualquier parpadeo activo
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkSelected());
    }

    private IEnumerator BlinkSelected()
    {
        while (true)
        {
            GetSelectedText().enabled = false; // Apagar la visibilidad
            yield return new WaitForSeconds(blinkingTime); // Esperar
            GetSelectedText().enabled = true; // Encender la visibilidad
            yield return new WaitForSeconds(blinkingTime); // Esperar
        }
    }

    private TMPro.TextMeshProUGUI GetSelectedText()
    {
        switch (currentIndex)
        {
            case 0: return casilla1;
            case 1: return casilla2;
            case 2: return casilla3;
            default: return casilla1;
        }
    }

    private void UpdateDisplay()
    {
        casilla1.text = initials[0].ToString();
        casilla2.text = initials[1].ToString();
        casilla3.text = initials[2].ToString();
    }

    public void SaveInitials()
    {
        // Convertir las iniciales a un string
        string finalInitials = new string(initials);
       
        // Obtener la puntuación actual del DataManager
        int finalScore = DataManager.Instance.score;

        // Crear un nuevo ScoreRecord con las iniciales y la puntuación
        HighScore newEntry = new HighScore(finalInitials, finalScore);

        // Guardar el nuevo HighScore en la lista de máximas puntuaciones
        DataManager.Instance.AddHighScore(newEntry);

        Debug.Log("Record entered: " + finalInitials + " " + finalScore);

        // Opcional: Regresar a la escena principal o mostrar la tabla de puntuaciones
        // SceneManager.LoadScene("HighScoresScene");
    }
}
