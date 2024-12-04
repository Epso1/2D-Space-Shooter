using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoresManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataManager.Instance.PrintHighScores();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
