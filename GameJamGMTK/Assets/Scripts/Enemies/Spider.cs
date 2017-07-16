using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spider : EnemiesScript {

	public float damage;
	NavMeshAgent agentNavig;
	protected override void Start () {
		base.Start();
		currentState = (int)states.wandering;
		destination = Wander();
		isWandering=true;
		agentNavig = GetComponent<NavMeshAgent>();
		stateFromColor();
	}

	protected override void Update(){
		base.Update();
		chooseBehavior();
		if(isActive && currentState != (int)states.idle){
			agentNavig.SetDestination(destination);
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

	public override void grabEnemy(){
		agentNavig.speed=0f;
		agentNavig.enabled=false;
		base.grabEnemy();
	}

//ETAT_________________________________________________________________________

	protected override void chooseBehavior(){
		if(!isActive)
			return;
		switch(currentState){
			case (int)states.wandering:
				agentNavig.speed= calmMoveSpeed;
				if(!isWandering){
					destination = Wander();
					isWandering = true;
				}
                break;
			case (int)states.flee: 
				agentNavig.speed= angryMoveSpeed;
				destination = Flee(player.transform.position);
				break;
			case (int)states.seek:
				agentNavig.speed = angryMoveSpeed;
				destination = Seek(player.transform.position);
				break;
		}
	}
	
	public void stateFromColor(){
		switch(mobColor){
			case colorMob.blue:
				enemyIngredientType = RecipeController.Ingredients.Spider_Blue;
				StartCoroutine(BlueState());
				break;
			case colorMob.green:
				enemyIngredientType = RecipeController.Ingredients.Spider_Green;
				int choose = (int)Random.Range(0f,9f);
				if(choose<5)
					StartCoroutine(RedState());	
				else
					StartCoroutine(BlueState());
			break;
			case colorMob.red:
				enemyIngredientType = RecipeController.Ingredients.Spider_Red;
				StartCoroutine(RedState());
				break;
			case colorMob.yellow:
				enemyIngredientType = RecipeController.Ingredients.Spider_Yellow;
				StartCoroutine(YellowState());
			break;
		}
	}

	IEnumerator BlueState() {
        while(isActive) {
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
            int rand = Random.Range(0, 10);

			if(distanceFromPlayer<=maxDistanceFromPlayer){
				//fuite 
				isWandering=false;
				previousState = currentState;
				currentState = (int)states.flee;
			}else if(currentState == (int)states.flee&& distanceFromPlayer>maxDistanceFromPlayer){
				//retour au wander après la fuite
				previousState = currentState;
				currentState = (int)states.wandering;
				isWandering=true;
			}
            else if (rand < 5 && (currentState == (int)states.wandering || currentState != (int)states.idle)) {
				//pause dans le wander
                previousState = currentState;
                currentState = (int)states.idle;
				isWandering=false;
                yield return new WaitForSeconds(Random.Range(2.0f,3.0f));
                currentState = previousState;
            } 
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
			}else if(currentState == (int)states.seek && distanceFromPlayer>maxDistanceFromPlayer){
				//s'éloigne trop de son point de spawn
				previousState = currentState;
				currentState = (int)states.wandering;
				isWandering=true;
			}
            else if (rand < 6 && (currentState == (int)states.wandering || currentState != (int)states.idle)) {
				//phase idle ou aucun mouvement
                previousState = currentState;
                currentState = (int)states.idle;
                yield return new WaitForSeconds(Random.Range(3.0f,5.0f));
                currentState = previousState;
            } 
        }
    }

	IEnumerator YellowState(){
		 while(isActive) {
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
            int rand = Random.Range(0, 10);
			if (rand < 5 && (currentState == (int)states.wandering || currentState != (int)states.idle)) {
				//pause dans le wander
                previousState = currentState;
                currentState = (int)states.idle;
				isWandering=false;
                yield return new WaitForSeconds(Random.Range(2.0f,3.0f));
                currentState = previousState;
            } 
		 }

	}
}
