using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

	public Animator anim;

	public GameObject player;
	public float playerDistance;
	public float smashAttackDistance;
	public float swipeAttackDistance;

	[SerializeField]
	protected float smashRadius;
	[SerializeField]
	protected float swipeRadius;

	bool canIAttack = true;

	public void OnDrawGizmos() {

		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere (transform.position, smashRadius);

		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere (transform.position, swipeRadius);
	}

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		playerDistance = Vector3.Distance (player.transform.position, transform.position);
		if (canIAttack) {
			if (playerDistance < smashAttackDistance && playerDistance > swipeAttackDistance) {
				StartCoroutine ("SmashAttack", IsThePlayerNearMe ());
			}
			if (playerDistance < swipeAttackDistance) {
				StartCoroutine ("SwipeAttack", IsThePlayerNearMe ());
			}
		}
	}

	bool IsThePlayerNearMe () {
		if (playerDistance < smashAttackDistance) {
			return true;
		} else {
			return false;
		}
	}

	IEnumerator SmashAttack (bool canAttack) {
		canIAttack = false;
		if (canAttack) {
			anim.SetBool ("GroundSmash", true);
			yield return new WaitForSeconds (1.0f);
			anim.SetBool ("GroundSmash", false);
		}
		canIAttack = true;
	}

	IEnumerator SwipeAttack (bool canAttack) {
		canIAttack = false;
		if (canAttack) {
			anim.SetBool ("Swipe", true);
			yield return new WaitForSeconds (1.0f);
			anim.SetBool ("Swipe", false);
		}
		canIAttack = true;
	}

	void TakeDamage () {
		anim.SetTrigger ("TakeDamage");
	}

	void OnCollisionEnter(Collider collider) {
		if (collider.transform.tag == "Player") {
			collider.transform.SendMessage ("TakeDamage");
		}
	}
}
