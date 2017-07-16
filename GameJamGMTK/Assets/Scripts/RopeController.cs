using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour {

    public Transform hand;
    public Transform rest;

    private Vector3 scale0;

    // Use this for initialization
    void Start () {
        scale0 = transform.localScale;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        var pA = hand.position;
        var pB = rest.position;
        transform.position = (pA + pB) / 2; // place the cube in the middle of A-B
        transform.LookAt(pB); // make it look to ballB position
                              // adjust cube length so it will have its ends at the sphere centers
        var scale = scale0;
        scale.z = scale0.z * Vector3.Distance(pA, pB);
        // stretch it in the direction it's looking
        transform.localScale = scale;
    }
}
