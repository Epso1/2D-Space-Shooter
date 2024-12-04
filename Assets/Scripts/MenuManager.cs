using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;
using static UnityEditor.Timeline.TimelinePlaybackControls;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource UIAudioSource;
    [SerializeField] private AudioClip startGameSFX;
    [SerializeField] private string scene01Name = "Scene01";    
    [SerializeField] private Button[] buttons; // Lista de botones en el menú
    [SerializeField] private AudioClip changeSelectionSFX;
    private int selectedIndex = 0; // Índice del botón seleccionado
    private PlayerInput playerInput;
    private bool canChangeSelection = true;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void Update()
    {
        Vector2 input = playerInput.actions["Navigate"].ReadValue<Vector2>();

        if (input.y > 0)
        {
            ChangeSelection(-1); // Arriba
        }
        else if (input.y < 0)
        {
            ChangeSelection(1); // Abajo
        }
    }

    private void ChangeSelection(int direction)
    {
        // Cambia el índice del botón seleccionado
        selectedIndex = (selectedIndex + direction + buttons.Length) % buttons.Length;
    }
    public void Submit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            buttons[selectedIndex].onClick.Invoke();
        }              
    }

    public void StartGame()
    {
        StartCoroutine(StarGameEnum());
    }

    private IEnumerator StarGameEnum()
    {
        UIAudioSource.clip = startGameSFX;
        UIAudioSource.Play();
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(scene01Name);
    }
}
