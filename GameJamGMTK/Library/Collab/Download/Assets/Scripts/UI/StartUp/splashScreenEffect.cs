using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class splashScreenEffect : MonoBehaviour {

	public string nextScene;

	void Start () {
		StartCoroutine (startNextScene ());
	}

	void Update () {
	
	}

	IEnumerator startNextScene(){
		yield return new WaitForSeconds (3f);
		SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
	}
}
