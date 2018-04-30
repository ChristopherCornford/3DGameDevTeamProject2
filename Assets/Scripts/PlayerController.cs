using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[Header("Movement")]
	public float playerSpeed;
	public float turnSpeed;
	float horizontal;
	float vertical;
	float rotate;
	private Rigidbody rb;
	private Animator playerAnim;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		playerAnim = GetComponent<Animator> ();
	}


	void Update() {
		horizontal = Input.GetAxisRaw ("Horizontal");
		vertical = Input.GetAxisRaw ("Vertical");
		rotate = Input.GetAxisRaw ("Mouse X") * Time.deltaTime * turnSpeed;
	}
	// Update is called once per frame
	void FixedUpdate () {
		Move ();
		StartCoroutine ("Rotate");
	}

	void Move() {
		Vector3 moveDir = new Vector3 (horizontal, 0.0f, vertical).normalized * playerSpeed;
		transform.Translate(transform.position + moveDir * Time.fixedDeltaTime);
	}
	IEnumerator Rotate () {
		transform.Rotate (new Vector3 (0.0f, 0.0f, rotate));
		yield return null;
	}
	public void TakeDamage () {
		Debug.Log (name + " " + "took damage!");
		playerAnim.SetTrigger ("TakeDamage");
	}
}
