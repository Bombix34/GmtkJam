using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryUI : MonoBehaviour {
	public GameObject prefabImage;
	PlayerInventory playerInventory;

	public IngredientsPicts ingredientsPicts;

	// Use this for initialization
	void Start () {
		playerInventory = PlayerInventory.instance;
		EventManager.StartListening ("InventoryChange", updateInventoryDisplay);
	}
	
	private void updateInventoryDisplay(){
		foreach (Transform child in transform) {
			GameObject.Destroy(child.gameObject);
		}
		foreach (RecipeController.Ingredients child in playerInventory.getAllIngredients()) {
			GameObject go = Instantiate(prefabImage);
			go.transform.SetParent(transform);
			//go.GetComponent<Image>().color = Color.blue;//A changer plus tard pour une vrai image
			go.GetComponent<Image>().sprite = ingredientsPicts.picts.Where(a => a.refIngredient == child).First().image;
		}
	}
}
