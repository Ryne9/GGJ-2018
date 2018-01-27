using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    public enum MenuStatus { unfocused, startPrompt, menu, activate };

    public bool isPlayer1Focused;
    public bool isPlayer2Focused;
    public MenuStatus focusStatus;
    public float focusProx;
    public bool isFocused;
    public GameObject player1;
    public GameObject player2;
    public MachineController machineController;

	// Use this for initialization
	void Start () {
        isFocused = false;
        focusStatus = MenuStatus.unfocused;
        machineController = GetComponent<MachineController>();
	}
	
	// Update is called once per frame
	void Update () {
        switch (focusStatus) {
            case MenuStatus.startPrompt:
                if (isPlayer1Focused)
                    if (Input.GetButtonDown("P1_A"))
                    {
                        focusStatus = MenuStatus.menu;
                        isPlayer2Focused = false;
                        player1.GetComponent<Player1_Movement>().canMove = false;
                        break;
                    }                        
                if (isPlayer2Focused)
                    if (Input.GetButtonDown("P2_A"))
                    {
                        focusStatus = MenuStatus.menu;
                        isPlayer1Focused = false;
                        player2.GetComponent<Player2_Movement>().canMove = false;
                    }
                break;
            case MenuStatus.menu:
                if (isPlayer1Focused)
                {
                    if (Input.GetButtonDown("P1_X"))
                    {
                        machineController.UseButton("X");
                    }
                    else if (Input.GetButtonDown("P1_Y"))
                    {
                        machineController.UseButton("Y");
                    }
                    else if (Input.GetButtonDown("P1_B"))
                    {
                        machineController.UseButton("B");
                    }
                    else if (Input.GetButtonDown("P1_A"))
                    {
                        machineController.UseButton("A");
                    }
                }
                else
                {
                    if (Input.GetButtonDown("P2_X"))
                    {
                        machineController.UseButton("X");
                    }
                    else if (Input.GetButtonDown("P2_Y"))
                    {
                        machineController.UseButton("Y");
                    }
                    else if (Input.GetButtonDown("P2_B"))
                    {
                        machineController.UseButton("B");
                    }
                    else if (Input.GetButtonDown("P2_A"))
                    {
                        machineController.UseButton("A");
                    }
                }
                break;
            case MenuStatus.activate:
                break;
        }
        if (focusStatus == MenuStatus.startPrompt)
        {
            if (Input.GetButtonDown("P1_A"))
                focusStatus = MenuStatus.menu;
        }
	}

    public void PlayerAlert(bool player1, bool player2)
    {
        if (focusStatus == MenuStatus.unfocused || focusStatus == MenuStatus.startPrompt)
        {
            isPlayer1Focused = player1;
            isPlayer2Focused = player2;
        }        

        if ((isPlayer1Focused || isPlayer2Focused) && focusStatus == MenuStatus.unfocused)
            focusStatus = MenuStatus.startPrompt;
    }

    public int GetStatus()
    {
        return (int) focusStatus;
    }

    public void SetPlayers(GameObject player1, GameObject player2)
    {
        this.player1 = player1;
        this.player2 = player2;
    }

    public void Unfocus()
    {
        focusStatus = MenuStatus.unfocused;
        if (isPlayer1Focused)
            player1.GetComponent<Player1_Movement>().canMove = true;
        if (isPlayer2Focused)
            player2.GetComponent<Player2_Movement>().canMove = true;
    }
}
