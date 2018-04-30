using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour {

	private const float Y_ANGLE_MIN = -30.0f;
	private const float Y_ANGLE_MAX = 50.0f;

	private float rotateSpeed = 500f;

	private float currentY;


	// Update is called once per frame
	void Update () {
		currentY += Input.GetAxis ("Mouse Y") * rotateSpeed * Time.deltaTime;

		currentY = Mathf.Clamp (currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
	}
	void LateUpdate() {
		transform.Rotate (new Vector3 (currentY, 0.0f, 0.0f));
	}
}
