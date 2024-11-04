using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float reloadSceneTime = 3f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void ReloadScene()
    {
        StartCoroutine(ReloadSceneEnum());
    }

    private IEnumerator ReloadSceneEnum()
    {
        yield return new WaitForSeconds(reloadSceneTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
