using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class MutatorController : MonoBehaviour, MachineController {

    MenuController menuController;
    GameObject mainCamera;
    InventoryController inventoryManager;

	// Use this for initialization
	void Start () {
        menuController = GetComponent<MenuController>();
        mainCamera = GameObject.Find("Main Camera");
        inventoryManager = mainCamera.GetComponent<InventoryController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (menuController.focusStatus == MenuController.MenuStatus.activate)
        {
            //Insert button minigame stuff here.
        }
    }

    public void UseButton(string button)
    {
        switch (button)
        {
            case "A":
                menuController.focusStatus = MenuController.MenuStatus.activate;
                break;
            case "X":
                inventoryManager.AddBlips(-1);
                //Change two resources to T2
                break;
            case "Y":
                inventoryManager.AddBlips(-1);
                //Change two resources to T2
                break;
            case "B":
                inventoryManager.AddBlips(-1);
                //Change two resources to T2
                break;
        }
    }
}
