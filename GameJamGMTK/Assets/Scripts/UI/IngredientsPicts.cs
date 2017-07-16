using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IngredientsPicts : ScriptableObject
{
    [Serializable]
    public struct NamedImage {
        public RecipeController.Ingredients refIngredient;
        public Sprite image;
    }
    //public Dictionary<RecipeController.Ingredients, Texture2D> picts = new Dictionary<RecipeController.Ingredients, Texture2D>();
    [SerializeField]
    public List<NamedImage> picts = new List<NamedImage>();
}