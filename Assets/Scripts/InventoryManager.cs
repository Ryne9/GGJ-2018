using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class InventoryManager : MonoBehaviour {

    Inventory inventory;
    int maxResources = 10;
    int maxBlips = 50;

	// Use this for initialization
	void Start () {
        inventory = new Inventory(maxResources, maxBlips);
	}
	
	// Update is called once per frame
	void Update () {
        inventory.AddResource(1);
	}
}
