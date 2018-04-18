using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


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
		velocity = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized * playerSpeed;
	}
	// Update is called once per frame
	void FixedUpdate () {
		Move ();
	}

	void Move() {

		rb.MovePosition (rb.position + velocity * Time.fixedDeltaTime);
	}
}
