using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public Camera cam;
	private Rigidbody rb;
	GameObject pauseScreen;
	Animator pauseAnim;
	bool pauseScreenActive = false;
	public ItemHolder itemHolder;
	[Header("Movement")]
	public float playerSpeed;
	public float turnSpeed;
	public float horizontal;
	public float vertical;
	float rotate;
	bool canAttack = true;
	private Animator playerAnim;
	[Header("Player Variables")]
	public int Health;

	[Header("Henry's Magical Pockets")]
	public GameObject henrySword;
	public GameObject henryAxe;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		playerAnim = GetComponent<Animator> ();
		pauseScreen = GameObject.FindGameObjectWithTag ("PauseScreen");
		pauseAnim = pauseScreen.transform.GetChild (1).GetComponent<Animator> ();
		pauseScreen.SetActive (false);
		henryAxe.SetActive(true);
		henryAxe.GetComponent<BoxCollider> ().enabled = false;
		henrySword.GetComponent<BoxCollider> ().enabled = false;
	}


	void Update() {
		horizontal = Input.GetAxisRaw ("Horizontal");
		vertical = Input.GetAxisRaw ("Vertical");
		rotate = Input.GetAxisRaw ("Mouse X") * Time.deltaTime * turnSpeed;
		Rotate ();
		if (Input.GetButtonDown ("Attack")) {
			Attack ();
		}
		if (Health <= 0) {
			Destroy (gameObject);
		}

		if (pauseScreenActive) {
			if (Input.anyKeyDown) {
				pauseScreen.SetActive (false);
				pauseScreenActive = false;
			}
		} 

		if (pauseScreenActive) {
			Time.timeScale = 0.0f;
		} else if (!pauseScreenActive) {
			Time.timeScale = 1.0f;
		}
	}
	void LateUpdate() {
		if (Input.GetButtonDown("Pause")) {
			if (!pauseScreenActive) {
				pauseScreen.SetActive (true);
				pauseAnim.SetTrigger ("isPaused");
				pauseScreenActive = true;
			}
		}
	}
	// Update is called once per frame
	void FixedUpdate () {
		Move ();
	}

	void Move() {
		Vector3 moveDir = new Vector3 (horizontal, 0.0f, vertical).normalized * playerSpeed;
		moveDir = cam.transform.TransformDirection (moveDir);
		moveDir.y = 0.0f;
		rb.MovePosition (rb.position + moveDir * Time.fixedDeltaTime);
		/*
		if (horizontal != 0 || vertical != 0) {
			playerAnim.SetBool ("isWalking", true);
		} else {
			playerAnim.SetBool ("isWalking", false);
		}
*/
	}
	void Rotate () {
		transform.Rotate (new Vector3 (0.0f, 0.0f, rotate));
	}

	public void Attack() {
		if (canAttack) {
			StartCoroutine ("AnimationBuffer", "Attack");
		}
	}

	public void TakeDamage () {
		Debug.Log (name + " " + "took damage!");
		Health--;
		StartCoroutine ("AnimationBuffer", "TakeDamage");
	}
	IEnumerator AnimationBuffer  (string animation) {
		switch (animation) {
		case "TakeDamage":
			yield return new WaitForSeconds (.25f);
			playerAnim.SetTrigger ("TakeDamage");
			break;
		case "PickUpItem":
			playerAnim.SetBool ("PickUpItem", true);
			yield return new WaitForSeconds (1.0f);
			playerAnim.SetBool ("PickUpItem", false);
			break;
		case "Attack":
			canAttack = false;
			henryAxe.GetComponent<BoxCollider> ().enabled = true;
			henrySword.GetComponent<BoxCollider> ().enabled = true;
			playerAnim.SetBool ("Attack", true);
			yield return new WaitForSeconds (1.0f);
			playerAnim.SetBool ("Attack", false);
			henryAxe.GetComponent<BoxCollider> ().enabled = false;
			henrySword.GetComponent<BoxCollider> ().enabled = false;
			canAttack = true;
			break;
		}
	}
	void OnTriggerEnter (Collider collider) {
		switch(collider.transform.tag){
		case "Sword":
			StartCoroutine ("AnimationBuffer", "PickUpItem");
			henryAxe.SetActive (false);
			henrySword.SetActive (true);
			Destroy (collider.gameObject);
			break;
		case "Fruit":
			StartCoroutine ("AnimationBuffer", "PickUpItem");
			Health++;
			Destroy (collider.gameObject);
			break;

	}
	}
	void PauseGame () {
		pauseScreen.SetActive (true);
		pauseScreenActive = true;
	}
}
