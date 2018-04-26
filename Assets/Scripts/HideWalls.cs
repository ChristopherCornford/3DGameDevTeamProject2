using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWalls : MonoBehaviour {

	[Range(0.1f, 5.0f)]
	public float hiddenWallRadius;

	public Transform player;
	public Vector3 playerPos;
	public List<Transform> hiddenObjects;
	public LayerMask layerMask;
	public RaycastHit ray;
	public RaycastHit hit;
	public Color visableColor;
	public Color hiddenColor;

	// Use this for initialization
	void Start () {
		hiddenObjects = new List<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		ShootRay ();
	}
	void ShootRay() {
		Vector3 direction = player.position - transform.position;
		float distance = direction.magnitude;
		Debug.DrawRay (transform.position, direction, Color.red);

		playerPos = player.transform.position;
		RaycastHit findObject;
		if (Physics.Raycast (transform.position, direction, out findObject, layerMask)) {
			if (findObject.transform.tag == "NonWalkableTerrain") {
				RaycastHit[] hits = Physics.SphereCastAll (playerPos, distance, direction, distance, layerMask);
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

						if (hits [j].transform == hiddenObjects [i]) {
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
	}
}
