using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StewController : MonoBehaviour {
	RecipeController recipeController;
	int recipeSize;
	
	void Start(){
		recipeController = GetComponent<RecipeController>();
		recipeController.newRecipe(recipeSize);
	}

	//Call with new added ingredients list
	private bool checkIngredient(RecipeController.Ingredients ingredient){
		if(recipeController.addIngredients(ingredient) && recipeController.isRecipeComplete()){
			recipeController.newRecipe(recipeSize);
			Scorer.instance.addScoreValue(2,1);
			return true;
		}
		return false;
	}
	void OnTriggerEnter(Collider other) {
       	if(other.tag == "Enemy"){
			print(other.gameObject.GetComponent<EnemiesScript>().enemyIngredientType);
			print(other.GetComponent<EnemiesScript>().getIsActive());
			if(!other.GetComponent<EnemiesScript>().getIsActive()){
				checkIngredient(other.gameObject.GetComponent<EnemiesScript>().enemyIngredientType);
			}
			Destroy(other.gameObject);
		}
    }

	public void setRecipeSize(int recipeSize){
		this.recipeSize = recipeSize;
	}

		
    private static StewController s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static StewController instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(StewController)) as StewController;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
				Debug.LogError("No StewController in scene");
				/* 
                GameObject obj = Instantiate(Resources.Load("PlayerController") as GameObject);
                s_Instance = obj.GetComponent<PlayerController>();*/
            }
            return s_Instance;
        }
	}
}
