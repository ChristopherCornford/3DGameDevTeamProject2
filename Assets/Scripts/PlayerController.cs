using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject worldCam;
	[Header("Movement")]
	public float playerSpeed;

	private Rigidbody rb;
	[SerializeField]
	private Vector3 velocity;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}


	void Update() {
		velocity = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized;
	}
	// Update is called once per frame
	void FixedUpdate () {
		Move ();
	}

	void Move() {

		rb.MovePosition (rb.position + velocity * Time.fixedDeltaTime);
		Vector3 moveDir = worldCam.transform.TransformDirection (velocity);
		transform.Translate (moveDir.normalized * playerSpeed);
	}
}
