using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Camera worldCam;
	[Header("Movement")]
	public float playerSpeed;
	float horizontal;
	float vertical;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}


	void Update() {
		horizontal = Input.GetAxisRaw ("Horizontal");
		vertical = Input.GetAxisRaw ("Vertical");

	}
	// Update is called once per frame
	void FixedUpdate () {
		Move ();
	}

	void Move() {
		Vector3 moveDir = new Vector3 (horizontal, 0.0f, vertical).normalized * playerSpeed;
		moveDir = Camera.main.transform.TransformDirection (moveDir);
		moveDir.y = 0.0f;
		rb.MovePosition (rb.position + moveDir * Time.fixedDeltaTime);
	}
}
