﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1_Movement : MonoBehaviour
{

	public float moveSpeed;
	public Vector3 direction;
	public Rigidbody rb;

	public float horiz;
	public float vert;

	public bool canMove = true;

	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody> ();
		moveSpeed = 18.0f;
	}

	// Update is called once per frame
	void Update()
	{
        //Debug.Log("P1: " + " A:" + Input.GetButton("P1_A") + " B:" + Input.GetButton("P1_B") + " X:" + Input.GetButton("P1_X") + " Y:" + Input.GetButton("P1_Y") + "\n" +
                  //"P2: " + " A:" + Input.GetButton("P2_A") + " B:" + Input.GetButton("P2_B") + " X:" + Input.GetButton("P2_X") + " Y:" + Input.GetButton("P2_Y") + "\n");

		if (canMove)
		{            
            horiz = Input.GetAxis("HorizontalPlayer1");
			vert = Input.GetAxis("VerticalPlayer1");

			horiz = Mathf.Abs (horiz) < 0.5f ? 0 : horiz;
			vert = Mathf.Abs (vert) < 0.5f ? 0 : vert;

			if (horiz != 0)
				horiz = horiz < 0 ? -1 : 1;

			if (vert != 0)
				vert = vert < 0 ? -1 : 1;

			direction = Vector3.Normalize(new Vector3(horiz, 0, vert));
			transform.LookAt(transform.position + direction);
		}
	}

	void FixedUpdate()
	{
		if (horiz != 0 || vert != 0)
		{
			rb.MovePosition (transform.position + direction * moveSpeed * Time.deltaTime);
		}
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}

    void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
