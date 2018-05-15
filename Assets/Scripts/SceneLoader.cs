using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour {

	public TMP_Text readyText;

	void Start () {
		readyText.enabled = false;
		StartCoroutine ("LoadLevel", 2);
	}
	public IEnumerator LoadLevel (int sceneIndex) {
		yield return null;

		AsyncOperation ops = SceneManager.LoadSceneAsync (sceneIndex);
		ops.allowSceneActivation = false;
		while (!ops.isDone) {
			Debug.Log (ops.progress);
			if (ops.progress >= 0.9f) {
				readyText.enabled = true;
				if (Input.anyKeyDown) {
					ops.allowSceneActivation = true;
				}
			}
		}
		yield return null;
	}
}
