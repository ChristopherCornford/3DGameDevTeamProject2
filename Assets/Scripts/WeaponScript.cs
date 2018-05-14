using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

	void OnTriggerEnter(Collider collider) {
		if (collider.transform.tag == "Enemy" || collider.transform.tag == "Boss") {
			collider.transform.SendMessage ("TakeDamage");
		}
	}
}
