using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class NavmeshBaking : MonoBehaviour {
	// Use this for initialization
	public void BakeNavMesh() {
		AddWalkableTerrain ();
		AddNonWalkableTerrain ();
	}
	void AddWalkableTerrain () {
		GameObject[] floorTiles = GameObject.FindGameObjectsWithTag ("WalkableTerrain");
		for (int i = 0; i < floorTiles.Length; i++) {
			GameObjectUtility.SetNavMeshArea (floorTiles [i], 0);
		}
	}
	void AddNonWalkableTerrain () {
		GameObject[] wallTiles = GameObject.FindGameObjectsWithTag ("NonWalkableTerrain");
		for (int i = 0; i < wallTiles.Length; i++) {
			GameObjectUtility.SetNavMeshArea (wallTiles [i], 1);
		}
	}

}
