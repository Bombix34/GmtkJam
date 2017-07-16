using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour {
	public GameObject prefabImage;
	public Camera cameraToLookAt;
	public IngredientsPicts ingredientsPicts;
	public RecipeController recipeController;

	void Start(){
		EventManager.StartListening ("RecipeChange", updateRecipeDisplay);
		updateRecipeDisplay();
	}
	void Update(){
		Vector3 v = cameraToLookAt.transform.position - transform.position;
		v.x = v.z = 0.0f;
		transform.LookAt( cameraToLookAt.transform.position - v ); 
		transform.Rotate(0,180,0);
	}

	private void updateRecipeDisplay(){
		foreach (Transform child in transform) {
			GameObject.Destroy(child.gameObject);
		}
		foreach (RecipeController.Ingredients child in recipeController.getRecipeLeft()) {
			GameObject go = Instantiate(prefabImage);
			go.transform.SetParent(transform);
			go.transform.localPosition = new Vector3(go.transform.position.x,go.transform.position.y,0);
			go.transform.localRotation = Quaternion.identity;
			go.GetComponent<Image>().sprite = ingredientsPicts.picts.Where(a => a.refIngredient == child).First().image;
		}
	}
}
