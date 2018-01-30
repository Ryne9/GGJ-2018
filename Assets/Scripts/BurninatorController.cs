using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class BurninatorController : MonoBehaviour, MachineController {

    MenuController menuController;
    GameObject mainCamera;
    InventoryController inventoryManager;
    public AudioSource source;

	// Use this for initialization
	void Start () {
        menuController = GetComponent<MenuController>();
        mainCamera = GameObject.Find("Main Camera");
        inventoryManager = mainCamera.GetComponent<InventoryController>();
        GetComponent<PlayerProximityChecker>().triggerProx = 8;
        source = GameObject.Find("brn8r").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UseButton(string button, int row)
    {
        switch (button)
        {
            case "A":
                menuController.Unfocus();              
                break;
            case "X":
                if (inventoryManager.GetResource(row * 3) > 0)
                {
                    inventoryManager.AddBlips(1);
                    inventoryManager.RemoveResource(row * 3);
                    source.PlayOneShot(source.clip);
                    menuController.cleanupSlate();
                }
                break;
            case "Y":
                if (inventoryManager.GetResource(1 + (row * 3)) > 0)
                {
                    inventoryManager.AddBlips(1);
                    inventoryManager.RemoveResource(1 + (row * 3));
                    source.PlayOneShot(source.clip);
                    menuController.cleanupSlate();
                }
                break;
            case "B":
                if (inventoryManager.GetResource(2 + (row * 3)) > 0)
                {
                    inventoryManager.AddBlips(1);
                    inventoryManager.RemoveResource(2 + (row * 3));
                    source.PlayOneShot(source.clip);
                    menuController.cleanupSlate();
                }
                break;
        }
    }
}
