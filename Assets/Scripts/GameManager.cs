using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	NavmeshBaking navMesh;

	void Awake() {
		navMesh = GetComponent<NavmeshBaking> ();
	}

	// Use this for initialization
	void Start () {
		navMesh.BakeNavMesh ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
