using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTimerUI : MonoBehaviour {
	PlayerTimer playerTimer;

	float fadeTime;
	float refFadeTime = 1f;

	bool ascending = false;
	private Color colorFade = Color.red;

	private int timeBlockSize = 20; 
	public Text blockTimeDisplay;
	public Slider gaugeDisplay;   
	
	 Animator animator;
    

        
	// Use this for initialization
	void Start () {
		playerTimer = PlayerTimer.instance;
		fadeTime = refFadeTime;
		colorFade.a = 0;
		animator = GetComponent<Animator>();
		EventManager.StartListening("TimeLost", lostTime);
	}
	
	// Update is called once per frame
	void Update () {
		float timeLeft = playerTimer.getTimeLeft();
		
		blockTimeDisplay.text = ((int)Mathf.Abs(timeLeft/timeBlockSize)).ToString();
		gaugeDisplay.value = timeLeft%timeBlockSize/timeBlockSize;

		fading();
	}

	private void lostTime(){
		animator.SetTrigger("TimeLost");
	}

	private void fading(){
		if(ascending){
			fadeTime += Time.deltaTime;
		}	else	{
			fadeTime -= Time.deltaTime;
		}

		if(fadeTime >= 1){
			fadeTime = 1;
			ascending = false;
		}	else if(fadeTime <= 0){
			fadeTime = 0;
			ascending = true;
		}

		if(blockTimeDisplay.text == "0"){
			colorFade.a = fadeTime;
			blockTimeDisplay.color = colorFade;
		}
	}
}