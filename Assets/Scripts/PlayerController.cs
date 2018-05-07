﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public Camera cam;
	private Rigidbody rb;
	public ItemHolder itemHolder;
	[Header("Movement")]
	public float playerSpeed;
	public float turnSpeed;
	float horizontal;
	float vertical;
	float rotate;
	bool canPickUp = true;
	bool canAttack = true;
	private Animator playerAnim;
	[Header("Player Variables")]
	public int Health;

	[Header("Item References")]
	public Items sword;
	public Items axe;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		playerAnim = GetComponent<Animator> ();
		//itemHolder = GetComponentInChildren<ItemHolder> ();
		//itemHolder.currentItem = axe;
	}


	void Update() {
		horizontal = Input.GetAxisRaw ("Horizontal");
		vertical = Input.GetAxisRaw ("Vertical");
		rotate = Input.GetAxisRaw ("Mouse X") * Time.deltaTime * turnSpeed;
		Rotate ();
		if (Input.GetButtonDown ("Attack")) {
			Attack ();
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
			playerAnim.SetBool ("Attack", true);
			yield return new WaitForSeconds (1.0f);
			playerAnim.SetBool ("Attack", false);
			canAttack = true;
			break;
		}
	}
	void OnTriggerEnter (Collider collider) {
		switch(collider.transform.tag){
		case "Sword":
			StartCoroutine ("AnimationBuffer", "PickUpItem");
			itemHolder.currentItem = sword;
			itemHolder.SpawnItem (canPickUp);
			break;
		case "Fruit":
			StartCoroutine ("AnimationBuffer", "PickUpItem");
			Health++;
			Destroy (collider.gameObject);
			break;

	}
	}
}
