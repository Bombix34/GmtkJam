using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathArea : MonoBehaviour {
	Vector3 startPosition;
	Quaternion startRotation;
	void Start(){
		startPosition = PlayerController.instance.transform.position;
		startRotation = PlayerController.instance.transform.rotation;
	}
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player"){
			PlayerTimer.instance.removeTime(30);
			other.gameObject.transform.position = startPosition;
			other.gameObject.transform.rotation = startRotation;
			other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
		if(other.gameObject.tag == "Enemy"){
			Scorer.instance.addScoreValue(0, 1);
			Destroy(other.gameObject);
		}
	}
}
