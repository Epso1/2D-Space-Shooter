using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOverManager : MonoBehaviour
{
    [SerializeField] private AudioSource UIAudioSource;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
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
    private void Start()
    {
        UpdateSelection();
    }
    private void Update()
    {

    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        if (context.performed)
        {
            if (input.y > 0.5f)
            {
                ChangeSelection(-1); // Arriba
            }
            else if (input.y < -0.5f)
            {
                ChangeSelection(1); // Abajo
            }
        }

    }
    private void UpdateSelection()
    {
        foreach (Button button in buttons)
        {
            var images = button.GetComponentsInChildren<Image>();
            images[1].color = color0;
        }

        var images2 = buttons[selectedIndex].GetComponentsInChildren<Image>();
        images2[1].color = color1;
    }

    private void ChangeSelection(int direction)
    {
        // Cambia el índice del botón seleccionado
        selectedIndex = (selectedIndex + direction + buttons.Length) % buttons.Length;
        UIAudioSource.PlayOneShot(changeSelectionSFX);
        UpdateSelection();
    }
    public void Submit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            buttons[selectedIndex].GetComponent<Image>().color = color2;
            buttons[selectedIndex].onClick.Invoke();
        }
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
   
}

