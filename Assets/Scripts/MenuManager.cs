using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AudioSource UIAudioSource;
    [SerializeField] private AudioClip startGameSFX;
    [SerializeField] private string scene01Name = "Scene01";
    [SerializeField] private string topScoresSceneName = "TopScores";
    [SerializeField] private Button[] buttons; // Lista de botones en el men�
    [SerializeField] private AudioClip changeSelectionSFX;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioClip menuMusic;

    private Color color0, color1, color2;
    private int selectedIndex = 0; // �ndice del bot�n seleccionado

    private void Awake()
    {
        // Detectar y destruir el DataManager si existe
        GameObject dataManager = GameObject.FindWithTag("DataManager");
        if (dataManager != null)
        {
            Destroy(dataManager);
        }

        color2 = new Color(.784f, .784f, .784f);
        color1 = Color.white;
        color0 = Color.white;
        color0.a = 0f;
    }

    private void Start()
    {
        PlayBGMusic();
        UpdateSelection();
    }

    private void Update()
    {

    }
    private void PlayBGMusic()
    {
        musicAudioSource.clip = menuMusic;
        musicAudioSource.Play();
        StartCoroutine(FadeInMusicEnum());
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
        // Cambia el �ndice del bot�n seleccionado
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

    public void StartGame()
    {
        StartCoroutine(StartGameEnum());
    }

    private IEnumerator StartGameEnum()
    {
        UIAudioSource.clip = startGameSFX;
        UIAudioSource.Play();
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(scene01Name);
    }

    public void ToTopScores()
    {
        StartCoroutine(ToTopScoresEnum());
    }

    private IEnumerator ToTopScoresEnum()
    {

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(topScoresSceneName);
    }


    public IEnumerator FadeInMusicEnum()
    {
        musicAudioSource.volume = 0f;
        while (musicAudioSource.volume < 1)
        {
            musicAudioSource.volume += 0.01f;
            yield return new WaitForSeconds(0.05f);
        }
        musicAudioSource.volume = 1;
    }
    public void ApplicationQuit()
    {
        Application.Quit();
    }
}
