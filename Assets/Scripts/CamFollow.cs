using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start ()
	{
	}

	// LateUpdate is called after Update each frame

	void Update () {
	}
	void LateUpdate ()
	{
		transform.position = player.transform.position;
	}
}