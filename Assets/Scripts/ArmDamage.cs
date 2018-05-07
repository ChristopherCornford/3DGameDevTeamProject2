using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmDamage : MonoBehaviour {

	void OnCollisionEnter (Collider collider){
		if (collider.transform.tag == "Player") {
			collider.transform.SendMessage ("TakeDamage");
		}
	}
}
