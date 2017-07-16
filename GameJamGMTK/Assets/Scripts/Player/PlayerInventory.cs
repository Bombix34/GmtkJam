using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
	List<RecipeController.Ingredients> ingredients;

	// Use this for initialization
	void Start () {
		resetInventory();
	}

	public void addIngredient(RecipeController.Ingredients ingredient){
		ingredients.Add(ingredient);
		EventManager.TriggerEvent ("InventoryChange");
	}

	public void resetInventory(){
		ingredients = new List<RecipeController.Ingredients>();
		EventManager.TriggerEvent ("InventoryChange");
	}

	public List<RecipeController.Ingredients> getAllIngredients(){
		return ingredients;
	}

	private static PlayerInventory s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static PlayerInventory instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(PlayerInventory)) as PlayerInventory;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
				Debug.LogError("No PlayerInventory in scene");
				/* 
                GameObject obj = Instantiate(Resources.Load("PlayerController") as GameObject);
                s_Instance = obj.GetComponent<PlayerController>();*/
            }
            return s_Instance;
        }
	}
}
