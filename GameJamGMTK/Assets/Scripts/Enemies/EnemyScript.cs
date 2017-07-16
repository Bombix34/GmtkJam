using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemiesScript : MonoBehaviour {
	public RecipeController.Ingredients enemyIngredientType;//La référence ingrédient de l'ennemie a voir plus tard ^^
	protected GameObject player;
	protected float distanceFromPlayer;
	public float maxDistanceFromPlayer;
	protected Vector3 destination;
	protected bool isActive, isWandering, isFleeing;
	public bool test;

	[SerializeField]protected int currentState,
                                previousState;
	protected enum states { idle, wandering, flee, seek, returnToOrigin };

	public enum colorMob{yellow, red, blue, green};
	
	public colorMob mobColor;
	protected float moveSpeed;
	public float calmMoveSpeed;
	public float angryMoveSpeed;

	public Spawner spawner;

	protected virtual void Start () {
		isActive = true;
		player = PlayerController.instance.gameObject;
	}

	protected void calculateDistanceFromPlayer(){
		distanceFromPlayer = Mathf.Sqrt(Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.y - transform.position.y, 2)+ Mathf.Pow(player.transform.position.z - transform.position.z, 2));
	}
	
	protected virtual void Update () {
		if(test)
			grabEnemy();
		if(!isActive)
			return;
		calculateDistanceFromPlayer();
	}

	protected Vector3 Normalize(Vector3 v) {
        float length = Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2)+ Mathf.Pow(v.z, 2));
        Vector3 temp;

        temp.x = v.x / length;
        temp.y = v.y / length;
		temp.z = v.z / length;

        return temp;
    }

	public bool getIsActive(){
		return isActive;
	}

	protected virtual Vector3 Wander() {
		return new Vector3(this.transform.position.x + Random.Range(-3f, 3f), this.transform.position.y, this.transform.position.z + Random.Range(-3f, 3f));
	}

	protected virtual Vector3 Seek(Vector3 pos){
		Vector3 movingTo;
        movingTo.x = (pos.x - transform.position.x) * Mathf.Sign(transform.localScale.x);
        movingTo.y = pos.y - transform.position.y;
		movingTo.z = (pos.z - transform.position.z) * Mathf.Sign(transform.localScale.z);

		movingTo = Normalize(movingTo);

		return new Vector3(transform.position.x+movingTo.x, transform.position.y, transform.position.z+movingTo.z);
	}

	protected virtual Vector3 Flee(Vector3 pos){

		Vector3 movingTo;
        movingTo.x = (transform.position.x- pos.x) * Mathf.Sign(transform.localScale.x);
        movingTo.y = transform.position.y;
		movingTo.z = (transform.position.z-pos.z) * Mathf.Sign(transform.localScale.z);
		movingTo = Normalize(movingTo);

		return new Vector3(transform.position.x+movingTo.x, transform.position.y, transform.position.z+movingTo.z);
	}

	protected abstract void chooseBehavior();

	public virtual void grabEnemy(){
		isActive=false;
		Rigidbody body = GetComponent<Rigidbody>();
		if(spawner!=null)
			spawner.removeEnemy();
		body.useGravity=false;
		body.isKinematic=true;
		body.velocity=new Vector3(0f,0f,0f);
		GetComponent<CapsuleCollider>().enabled=false;
	}

	public void resetGravity(){
		moveSpeed =0f;
		GetComponent<Rigidbody>().useGravity=true;
		GetComponent<Rigidbody>().isKinematic=false;
		GetComponent<CapsuleCollider>().enabled=true;
	}

	public void setColor(colorMob toSet){
		mobColor = toSet;
	}
}
