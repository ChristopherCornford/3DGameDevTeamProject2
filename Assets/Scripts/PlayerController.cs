using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	[Header("Movement")]
	public float playerSpeed;

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Move ();
	}

	void Move() {

		float horizontalMovement = Input.GetAxisRaw ("Horizontal");
		float verticalMovement = Input.GetAxisRaw ("Vertical");

		Vector3 move = new Vector3 (-verticalMovement, 0.0f, horizontalMovement);
		rb.AddForce (move * playerSpeed);
	}
}
