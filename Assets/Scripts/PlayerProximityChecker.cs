using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProximityChecker : MonoBehaviour {

    public float player1Prox;
    public float player2Prox;
    public float triggerProx;
    public Transform player1;
    public Transform player2;

	// Use this for initialization
	void Start () {
        triggerProx = 3f;
	}
	
	// Update is called once per frame
	void Update () {
        player1Prox = Vector3.Distance(transform.position, player1.transform.position);
        player2Prox = Vector3.Distance(transform.position, player2.transform.position);
        if (player1Prox < triggerProx)
        {
            print("GET PLAYER 1 AWAY!!!!");
        }
        if (player2Prox < triggerProx)
        {
            print("GET PLAYER 2 AWAY!!!!");
        }
    }
}
