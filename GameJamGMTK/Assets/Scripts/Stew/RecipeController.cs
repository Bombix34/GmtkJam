using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeController : MonoBehaviour {

	public enum Ingredients
	{
		Spider_Blue, Spider_Red, Spider_Yellow, Spider_Green, Bird_Blue, Bird_Red, Bird_Yellow, Bird_Green
	}
	List<Ingredients> recipeIngredients;
	List<Ingredients> actualIngredients;



	//return true quand l'ingrédient est bien ajouté
	public bool addIngredients(Ingredients ingredient){
		if(recipeIngredients.Where(ing => ing == ingredient).Count() == actualIngredients.Where(ing => ing == ingredient).Count()) {
			return false;
		}	else	{
			actualIngredients.Add(ingredient);
			EventManager.TriggerEvent("RecipeChange");
			return true;
		}
	}

	public bool isRecipeComplete(){
		return recipeIngredients.Count == actualIngredients.Count;
	}

	public List<Ingredients> getRecipeLeft(){
		List<Ingredients> temporary = new List<Ingredients>(recipeIngredients);
		foreach (var item in actualIngredients)
		{
			temporary.Remove(item);
		}
		return temporary;
	}


	//Créer une nouvelle recette avec en paramètre le nombre d'ingrédients
	public void newRecipe(int ingredientsNumber){
		recipeIngredients = new List<Ingredients>();
		actualIngredients = new List<Ingredients>();

		var ingredients = System.Enum.GetNames(typeof(Ingredients));
		for (int i = 0; i < ingredientsNumber; i++)
		{
			recipeIngredients.Add((Ingredients) System.Enum.Parse( typeof( Ingredients ), ingredients[Random.Range(0, ingredients.Length)]));
		}
		EventManager.TriggerEvent("RecipeChange");
	}
}
