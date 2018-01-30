using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Over_Controller : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("P1_A") || Input.GetButtonDown("P2_A"))
        {
            MainMenu();
        }
        if (Input.GetButtonDown("P2_X") || Input.GetButtonDown("P2_X"))
        {
            QuitGame();
        }
	}

    public void MainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
