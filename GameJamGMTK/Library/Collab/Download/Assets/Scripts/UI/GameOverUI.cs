using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
	public GameObject endGamePanel;
	public Text RecipeDoneLabel;
	public Text RecipeDoneText;
	public Text RecipeDoneHS;
	public Text EnemyKilledLabel;
	public Text EnemyKilledText;
	public Text EnemyKilledHS;
	// Use this for initialization
	void Start () {
		EventManager.StartListening ("GameLost", display);
		RecipeDoneLabel.text = Scorer.instance.scores[2].name;
		EnemyKilledLabel.text = Scorer.instance.scores[0].name;
	}

	public void display(){
		updateStats();
		endGamePanel.SetActive(true);
	}

	public void undisplay(){
		endGamePanel.SetActive(false);
	}

	public void updateStats(){
		RecipeDoneText.text = ((int)Scorer.instance.getScore(2)).ToString();
		EnemyKilledText.text = ((int)Scorer.instance.getScore(0)).ToString();
		RecipeDoneHS.gameObject.SetActive(Scorer.instance.getScore(2) == Scorer.instance.getScore(3));
		EnemyKilledHS.gameObject.SetActive(Scorer.instance.getScore(0) == Scorer.instance.getScore(1));
	}
}
