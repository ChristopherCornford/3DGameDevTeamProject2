using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour {

	public TMP_Text readyText;
	AsyncOperation ops;
	bool isReadyToPlay;

	void Start () {
		readyText.enabled = false;
		StartCoroutine ("LoadLevel", 2);
	}
	void Update () {
		if (isReadyToPlay) {
			readyText.enabled = true;
			if (Input.anyKeyDown) {
				ops.allowSceneActivation = true;
			}
		}
	}
	IEnumerator LoadLevel (int sceneIndex) {
		yield return null;

		ops = SceneManager.LoadSceneAsync (sceneIndex);
		ops.allowSceneActivation = false;

		yield return new WaitForSeconds (7.5f);

		isReadyToPlay = true;

		yield return null;
	}
}
