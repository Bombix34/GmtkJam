using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {
	public float speed;
	private Vector3 originPosition;
	[SerializeField]
	public Vector3 MoveToRelative;

	private Vector3 finalPosition;

	private bool goingToFinal = true;
	// Use this for initialization
	
	void Start(){
		originPosition = transform.position;
		finalPosition = originPosition + MoveToRelative;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if((Vector3.Distance(transform.position, originPosition)<0.1 && !goingToFinal) || ((Vector3.Distance(transform.position, finalPosition)<0.1 && goingToFinal))){
			goingToFinal = !goingToFinal;
		}
		Vector3 heading;
		if(goingToFinal){
			heading = finalPosition - transform.position;
	

			//transform.Translate(direction * speed * Time.deltaTime);
		}	else	{
			heading = transform.position - finalPosition;
			/*
			var distance = heading.magnitude;
			var direction = heading / distance; */
			//transform.Translate(direction * speed * Time.deltaTime);
		}
		var distance = heading.magnitude;
		var direction = heading / distance;
		GetComponent<Rigidbody>().MovePosition(transform.position + direction * speed * Time.deltaTime);
	}


	void OnDrawGizmosSelected(){
		Gizmos.DrawCube(MoveToRelative+transform.position, Vector3.one);
	}
}
