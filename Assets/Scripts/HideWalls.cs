using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWalls : MonoBehaviour {


	public Transform player;

	public List<Transform> hiddenObjects;

	public LayerMask layerMask;
	public Ray ray;
	public Color visableColor;
	public Color hiddenColor;

	// Use this for initialization
	void Start () {
		hiddenObjects = new List<Transform>();
		ray = Camera.main.ScreenPointToRay (transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		ShootRay ();
	}
	void ShootRay() {
		Vector3 direction = player.position - transform.position;
		float distance = direction.magnitude;
		Debug.DrawRay (transform.position, direction, Color.red);
		RaycastHit[] hits = Physics.RaycastAll (transform.position, direction, distance, layerMask);

		for (int i = 0; i < hits.Length; i++) {
			Transform currentHit = hits [i].transform;

			if (!hiddenObjects.Contains (currentHit)) {
				hiddenObjects.Add (currentHit);
				currentHit.GetComponent<MeshRenderer> ().material.color = hiddenColor;
			}
		}
		for (int i = 0; i < hiddenObjects.Count; i++) {

			bool isHit = false;

			for (int j = 0; j < hits.Length; j++) {

				if (hits[j].transform == hiddenObjects[i]) {
					isHit = true;
					break;
				}
			}
			if (!isHit) {
				Transform wasHidden = hiddenObjects [i];
				wasHidden.GetComponent<MeshRenderer> ().material.color = visableColor;
				hiddenObjects.RemoveAt (i);
				i--;
			}
		}
	}
}
