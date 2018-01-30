using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProximityChecker : MonoBehaviour {

    public float player1Prox;
    public float player2Prox;
    public float triggerProx;
    public Transform player1;
    public Transform player2;
    public MenuController menuController;

	// Use this for initialization
	void Start () {
        triggerProx = 12f;
        menuController = GetComponent<MenuController>();
        menuController.SetPlayers(player1.gameObject, player2.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        player1Prox = Vector3.Distance(transform.position, player1.transform.position);
        player2Prox = Vector3.Distance(transform.position, player2.transform.position);
        menuController.PlayerAlert(player1Prox < triggerProx, player2Prox < triggerProx);
    }
}
