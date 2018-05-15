using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

	public Animator anim;
	public float currentAlpha;

	public GameObject player;
	public int Health;
	public float playerDistance;
	public float smashAttackDistance;
	public float swipeAttackDistance;

	[SerializeField]
	protected float smashRadius;
	[SerializeField]
	protected float swipeRadius;

	bool canIAttack = true;
	bool canBeDamaged = true;

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
		if (IsThePlayerNearMe ()) {
			//LookAtPlayer ();
		}
		if (canIAttack) {
			if (playerDistance < smashAttackDistance && playerDistance > swipeAttackDistance) {
				StartCoroutine ("SmashAttack", IsThePlayerNearMe ());
			}
			if (playerDistance < swipeAttackDistance) {
				StartCoroutine ("SwipeAttack", IsThePlayerNearMe ());
			}
		}
		if (Health == 0) {
			Die ();
		}
	}
/*	void LookAtPlayer () {
		Vector3 playerPos = player.transform.position;
		Vector3 enemyPos = transform.position;
		Vector3 delta = new Vector3 (playerPos.x - enemyPos.x, 0.0f, playerPos.z - enemyPos.z);
		Quaternion rotation = Quaternion.LookRotation (delta);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * 0.5f);
	}*/

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
		if (canBeDamaged) {
			StartCoroutine ("Damage");
		}
	}

	IEnumerator Damage () {
		canBeDamaged = false;
		anim.SetTrigger ("TakeDamage");
		Health--;
		yield return new WaitForSeconds(1.0f);
		canBeDamaged = true;		
	}
	void Die () {
		anim.SetBool ("isDead", true);
		Destroy (gameObject);
	}
}
