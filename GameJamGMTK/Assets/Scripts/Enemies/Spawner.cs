using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public enum enemyType{spider, bird};
	public enemyType enemyToSpawn;
	public int maxEnemy;

	private int enemySpawned;

	public GameObject[] enemies;
	private float chrono;
	void Start () {
		chrono = Random.Range(3f, 5f);
	}

	void Update () {
		if(enemySpawned==maxEnemy)
			return;
		chrono-=Time.deltaTime;
		if(chrono<=0){
			spawnEnemy();
			chrono = Random.Range(3f,5f);
		}
	}

	private void spawnEnemy(){
		GameObject toInstantiate;
		toInstantiate = Instantiate (enemies[(int)enemyToSpawn], getRandomOuterPosition(), Quaternion.identity) as GameObject;
		toInstantiate.GetComponent<EnemiesScript>().spawner = this;
		enemySpawned++;
	}

	public void removeEnemy(){
		enemySpawned--;
	}

	private Vector3 getRandomOuterPosition(){
		return new Vector3(transform.position.x + Random.Range(-2f,2f),transform.position.y ,transform.position.z + Random.Range(-2f,2f));
	}
}
