using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowerUpManager : MonoBehaviour
{
    private static PowerUpManager instance;

    public static PowerUpManager Instance { get { return instance; } }

    public bool isMissileActive = false;
    public bool isMultipleActive = false;
    public float currentSpeed = 2f;

    private Player player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
       
    }

    public void SavePowerUpData()
    {
        isMissileActive = player.missileEnabled;
        isMultipleActive = player.multipleEnabled;
        currentSpeed = player.speed;
    }

    public void ResetPowerUpData()
    {
        isMissileActive = false;
        isMultipleActive = false;
        currentSpeed = 2f;
    }

    public void LoadPowerUpData()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.missileEnabled = isMissileActive;
        player.multipleEnabled = isMultipleActive;
        player.speed = currentSpeed;
    }

}
