using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	NavmeshBaking navMesh;
	//GameObject[] floorTiles;


	void Awake() {
		navMesh = GetComponent<NavmeshBaking> ();
	}

	// Use this for initialization
	void Start () {
		navMesh.BakeNavMesh ();
		//floorTiles = GameObject.FindGameObjectsWithTag ("WalkableTerrain");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
