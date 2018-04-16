using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MapGeneration))]
public class MapGeneratorButton : Editor{
	public override void OnInspectorGUI() {
		
		base.OnInspectorGUI ();

		MapGeneration mapGeneration = (MapGeneration)target;

		if (GUILayout.Button ("Generate Map")) {
			mapGeneration.GenerateMap ();
		}
	}
}
