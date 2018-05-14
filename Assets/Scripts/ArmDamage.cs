using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmDamage : MonoBehaviour {

	void OnCollisionEnter (Collision collider){
		if (collider.transform.tag == "Player") {
			collider.transform.SendMessage ("TakeDamage");
		}
	}
}
