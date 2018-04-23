using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {

	public GameObject player;
	public float cameraRotateSpeed;
	bool lookAtPlayer = false;
	bool rotateAroundPlayer = true;
	float cameraTurnValue;

	private Vector3 offset;

	// Use this for initialization
	void Start () 
	{
		offset = transform.position - player.transform.position;
	}

	// LateUpdate is called after Update each frame

	void Update () {
		cameraTurnValue = Input.GetAxisRaw ("CameraRotation");
	}
	void LateUpdate () 
	{
		if (rotateAroundPlayer) {
			Quaternion camTurnAngle = Quaternion.AngleAxis (-cameraTurnValue * cameraRotateSpeed, Vector3.up);
			offset = camTurnAngle * offset;
		}
		transform.position = player.transform.position + offset;

		if (lookAtPlayer || rotateAroundPlayer) {
			transform.LookAt (player.transform);
		}
	}
}