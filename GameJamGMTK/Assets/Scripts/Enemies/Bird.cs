using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : EnemiesScript {
//NE FONCTIONNE PAS AVEC LES NAVMESH CAR ENNEMIS VOLANT


	public float damage;
	[SerializeField] private Vector3 direction;
	private Vector3 ancientDirection;

	private Vector3 initPosition;

	protected override void Start(){
		base.Start();
		moveSpeed = calmMoveSpeed;
		initPosition = this.transform.position;
		currentState = (int)states.wandering;
		destination = Wander();
		direction = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f,1.0f));
		ancientDirection = direction;
		stateFromColor();
	}

	protected override void Update(){
		base.Update();
		chooseBehavior();
	}

	protected override Vector3 Wander(){
		direction.x = ancientDirection.x + Random.Range(-0.1f, 0.1f);
        direction.y = ancientDirection.y + Random.Range(-0.1f, 0.1f);
		direction.z = ancientDirection.z + Random.Range(-0.1f, 0.1f);

        return Normalize(direction);
	}

	protected override Vector3 Seek(Vector3 pos) {
        Vector3 movingTo;
        movingTo.x = (pos.x - transform.position.x) * Mathf.Sign(transform.localScale.x);
        movingTo.y = pos.y - transform.position.y;
		movingTo.z = (pos.z - transform.position.z) * Mathf.Sign(transform.localScale.z);

		movingTo = Normalize(movingTo);
		
		//transform.rotation = Quaternion.LookRotation(initPosition-transform.position);
        return movingTo;
    }

	protected override Vector3 Flee(Vector3 pos) {
       	Vector3 movingTo;
        movingTo.x = (transform.position.x- pos.x) * Mathf.Sign(transform.localScale.x);
        movingTo.y = transform.position.y;
		movingTo.z = (transform.position.z-pos.z) * Mathf.Sign(transform.localScale.z);

		movingTo = Normalize(movingTo);
		
		//transform.rotation = Quaternion.LookRotation(initPosition-transform.position);
        return movingTo;
    }

	private float getDistanceFromInitPoint(){
		return Mathf.Sqrt(Mathf.Pow(initPosition.x - transform.position.x, 2) + Mathf.Pow(initPosition.y - transform.position.y, 2)+ Mathf.Pow(initPosition.z - transform.position.z, 2));
	}

	void resetAncientDirection(){
		ancientDirection = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f,1.0f));
	}


//ETAT_________________________________________________________________________

	protected override void chooseBehavior(){
		if(!isActive)
			return;
		switch(currentState){
			case (int)states.wandering:
				moveSpeed = calmMoveSpeed;
				direction = Wander();
				transform.Translate(direction * moveSpeed * Time.deltaTime);
                break;
			case (int)states.returnToOrigin:
				moveSpeed = angryMoveSpeed;
				direction = Seek(initPosition);
				transform.Translate(direction * moveSpeed * Time.deltaTime);
                break;
			case (int)states.seek:
				moveSpeed = angryMoveSpeed;
				direction = Seek(player.transform.position);
				transform.Translate(direction * moveSpeed * Time.deltaTime);
				break;
			case (int)states.flee:
				moveSpeed = angryMoveSpeed;
				direction = Flee(player.transform.position);
				transform.Translate(direction * moveSpeed * Time.deltaTime);
				break;
		}
	}
	
	public void stateFromColor(){
		switch(mobColor){
			case colorMob.blue:
				enemyIngredientType = RecipeController.Ingredients.Bird_Blue;
				StartCoroutine(BlueState());
				break;
			case colorMob.green:
				enemyIngredientType = RecipeController.Ingredients.Bird_Green;
				int choose = (int)Random.Range(0f,9f);
				if(choose<5)
					StartCoroutine(RedState());	
				else
					StartCoroutine(BlueState());
			break;
			case colorMob.red:
				enemyIngredientType = RecipeController.Ingredients.Bird_Red;
				StartCoroutine(RedState());
				break;
			case colorMob.yellow:
				enemyIngredientType = RecipeController.Ingredients.Bird_Yellow;
				StartCoroutine(YellowState());
			break;
		}
	}

	void OnCollisionEnter(Collision other)
    {	
		if(other.gameObject.tag =="Player"){
			if(!isActive)
				return;
			PlayerTimer.instance.removeTime(damage);
        	Destroy(this.gameObject);
		}
		if(other.gameObject.tag == "Enemy" && !isActive){
			Scorer.instance.addScoreValue(0,2);
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
    }

	IEnumerator RedState() {
        while(isActive) {
            yield return new WaitForSeconds(Random.Range(0.5f, 1.0f));
            int rand = Random.Range(0, 10);

			if(distanceFromPlayer<=maxDistanceFromPlayer){
				//poursuite du personnage
				previousState=currentState;
				currentState = (int) states.seek;
			}else if(getDistanceFromInitPoint()>7f){
				//s'éloigne trop de son point de spawn
				previousState=currentState;
				currentState = (int)states.returnToOrigin;
			}else if(currentState == (int)states.returnToOrigin && getDistanceFromInitPoint()<3f){
				//retour dans sa zone de confort
				resetAncientDirection();
				previousState = currentState;
				direction = Wander();
				currentState = (int)states.wandering;
			}
            else if (rand < 6 && (currentState == (int)states.wandering || currentState != (int)states.idle)) {
				//phase idle ou aucun mouvement
                previousState = currentState;
                currentState = (int)states.idle;
				resetAncientDirection();
                yield return new WaitForSeconds(Random.Range(3.0f,5.0f));
                currentState = previousState;
            } 
        }
    }

	IEnumerator BlueState() {
        while(isActive) {
            yield return new WaitForSeconds(Random.Range(0.5f, 1.0f));
            int rand = Random.Range(0, 10);

			if(distanceFromPlayer<=maxDistanceFromPlayer){
				//senfuit
				previousState=currentState;
				currentState = (int) states.flee;
			}else if(getDistanceFromInitPoint()>7f && currentState!=(int)states.flee){
				//s'éloigne trop de son point de spawn
				previousState=currentState;
				currentState = (int)states.returnToOrigin;
			}else if(currentState == (int)states.returnToOrigin && getDistanceFromInitPoint()<3f){
				//retour dans sa zone de confort
				resetAncientDirection();
				previousState = currentState;
				direction = Wander();
				currentState = (int)states.wandering;
			}
            else if (rand < 6 && (currentState == (int)states.wandering || currentState != (int)states.idle)) {
				//phase idle ou aucun mouvement
                previousState = currentState;
                currentState = (int)states.idle;
				resetAncientDirection();
                yield return new WaitForSeconds(Random.Range(3.0f,5.0f));
                currentState = previousState;
            } 
        }
    }

	IEnumerator YellowState(){
		 while(isActive) {
            yield return new WaitForSeconds(Random.Range(0.5f, 1.0f));
            int rand = Random.Range(0, 10);
			
			if(getDistanceFromInitPoint()>7f){
				//s'éloigne trop de son point de spawn
				previousState=currentState;
				currentState = (int)states.returnToOrigin;
			}else if(currentState == (int)states.returnToOrigin && getDistanceFromInitPoint()<3f){
				//retour dans sa zone de confort
				resetAncientDirection();
				previousState = currentState;
				direction = Wander();
				currentState = (int)states.wandering;
			}
            else if (rand < 6 && (currentState == (int)states.wandering || currentState != (int)states.idle)) {
				//phase idle ou aucun mouvement
                previousState = currentState;
                currentState = (int)states.idle;
				resetAncientDirection();
                yield return new WaitForSeconds(Random.Range(3.0f,5.0f));
                currentState = previousState;
            } 
        }

	}

}
