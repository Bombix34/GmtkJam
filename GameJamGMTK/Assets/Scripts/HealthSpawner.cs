using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawner : MonoBehaviour {

    public int howManyOrbToSpawn = 5;
    public GameObject theOrb;
	// Use this for initialization
	void Start () {
        spawnTheOrbs();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void spawnTheOrbs()
    {
        for (int i = 0; i < howManyOrbToSpawn; i++)
        {
            Instantiate(theOrb, transform.position, Quaternion.identity);
        }
    }
}
