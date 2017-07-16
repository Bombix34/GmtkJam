using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimer : MonoBehaviour {
	private bool gameLost = false;
	private float playerTimeLeft;

    public GameObject cameraAnchor;
	// Use this for initialization
	void Start () {
		playerTimeLeft = 25f;
	}
	
	// Update is called once per frame
	void Update () {
		if(gameLost){
			return;
		}
		playerTimeLeft -= Time.deltaTime;
		if(playerTimeLeft <= 0){
			//END GAME
			/*
			Time.timeScale = 0;
			EventManager.TriggerEvent("GameLost");
			gameLost = true; */
		}
	}

	public void addTimeLeft(float timeValue){
        EventManager.TriggerEvent("TimeGain");
        playerTimeLeft += timeValue;
        Debug.Log("wtf");
    }

	public void removeTime(float timeValue){
        cameraAnchor.GetComponent<CameraController>().startCameraShake();
		playerTimeLeft -= timeValue;
		EventManager.TriggerEvent("TimeLost");
	}

	public float getTimeLeft(){
		return playerTimeLeft;
	}

	private static PlayerTimer s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static PlayerTimer instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(PlayerTimer)) as PlayerTimer;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
				Debug.LogError("No playertimer in scene");
				/* 
                GameObject obj = Instantiate(Resources.Load("PlayerController") as GameObject);
                s_Instance = obj.GetComponent<PlayerController>();*/
            }
            return s_Instance;
        }
	}
}
