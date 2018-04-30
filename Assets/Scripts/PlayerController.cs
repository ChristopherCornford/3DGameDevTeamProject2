using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public Camera worldCam;
	[Header("Movement")]
	public float playerSpeed;
	public float turnSpeed;
	float horizontal;
	float vertical;
	float rotate;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}


	void Update() {
		horizontal = Input.GetAxisRaw ("Horizontal");
		vertical = Input.GetAxisRaw ("Vertical");
		rotate = Input.GetAxisRaw ("Mouse X") * Time.deltaTime * turnSpeed;
	}
	// Update is called once per frame
	void FixedUpdate () {
		Move ();
		Rotate ();
	}

	void Move() {
		Vector3 moveDir = new Vector3 (horizontal, 0.0f, vertical).normalized * playerSpeed;
		moveDir = Camera.main.transform.TransformDirection (moveDir);
		moveDir.y = 0.0f;
		rb.MovePosition (rb.position + moveDir * Time.fixedDeltaTime);
	}
	void Rotate () {
		transform.Rotate (new Vector3 (0.0f, 0.0f, rotate));
	}
}
