using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TopScoresController : MonoBehaviour
{
    [SerializeField] string mainMenuSceneName = "MainMenu";
    void Start()
    {
        Invoke("ToMainMenu",5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
}
