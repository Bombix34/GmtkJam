using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthOrb : MonoBehaviour {

    public int healthValue = 5;
    public float minimumDistance = 1f;
    public float maximumDistance = 2f;
    public float speedTravel = 1f;

    Vector3 randomSpawnPosition;
    GameObject thePlayer;
    bool travel = false;

	// Use this for initialization
	void Start () {
        thePlayer = GameObject.FindGameObjectWithTag("Player");
        randomSpawnPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f));
        randomSpawnPosition = randomSpawnPosition.normalized;
        randomSpawnPosition = randomSpawnPosition * Random.Range(minimumDistance, maximumDistance);
        randomSpawnPosition = transform.position + randomSpawnPosition;
        Invoke("startTravelilng", 2f);
	}
	
	// Update is called once per frame
	void Update () {
		if(travel)
        {
            transform.position = Vector3.MoveTowards(transform.position, thePlayer.transform.position, speedTravel);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, randomSpawnPosition, speedTravel);
        }
	}

    void startTravelilng()
    {
        travel = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            thePlayer.GetComponent<PlayerTimer>().addTimeLeft(healthValue);
            Destroy(gameObject);
        }
    }
}
