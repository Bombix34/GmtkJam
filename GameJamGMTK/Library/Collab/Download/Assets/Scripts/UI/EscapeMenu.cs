using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour {
	public GameObject escapePanel;

	bool gameLost = false;
	void Start(){
		EventManager.StartListening ("GameLost", forceGameLost);
	}
	void Update(){
		if(!gameLost && Input.GetKeyDown(KeyCode.Escape)){
			escapePanel.SetActive(!escapePanel.activeInHierarchy);
			if(escapePanel.activeInHierarchy){
				Time.timeScale = 0;
			}	else	{
				Time.timeScale = 1;
			}
		}
	}
	public void forceGameLost(){
		gameLost = true;
		escapePanel.SetActive(true);
	}

	public void newGame(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
	}

	public void returnToMainMenu(){
		SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
	}

	public void quit(){
		Application.Quit();
	}
}
