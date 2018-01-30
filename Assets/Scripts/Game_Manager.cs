using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("P1_A") || Input.GetButtonDown("P2_A"))
        {
            StartGame();
        } 
        if (Input.GetButtonDown("P1_X") || Input.GetButtonDown("P2_X"))
        {
            ExitGame();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("basiclevel");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
