using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class BurninatorController : MonoBehaviour, MachineController {

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
		
	}

    public void UseButton(string button)
    {
        switch (button)
        {
            case "A":
                menuController.Unfocus();              
                break;
            case "X":
                inventoryManager.AddBlips(1);
                //Remove some resource
                break;
            case "Y":
                inventoryManager.AddBlips(1);
                //Remove some resource
                break;
            case "B":
                inventoryManager.AddBlips(1);
                //Remove some resource
                break;
        }
    }
}
