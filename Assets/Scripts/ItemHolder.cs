using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour {

	public bool spawnItem;
	public Items currentItem;


	// Update is called once per frame
	void Update () {
		SpawnItem (spawnItem);
	}
	void SpawnItem (bool readyToSpawn) {
		if (readyToSpawn) {
			GameObject heldItem = Instantiate (currentItem.prefab, transform.position, Quaternion.identity) as GameObject;
			heldItem.transform.SetParent (transform); 	
			spawnItem = false;
			readyToSpawn = false;
		}
	}
}
