using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class TransmitterController : MonoBehaviour, MachineController {

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
                //Handle mission turn-in
                break;
            case "Y":
                //Handle mission turn-in
                break;
            case "B":
                //Handle mission turn-in
                break;
        }
    }
}
