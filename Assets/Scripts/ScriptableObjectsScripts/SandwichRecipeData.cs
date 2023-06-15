using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "NewSandwichRecipeData", menuName = "SandwichData/SandwichRecipeData")]
public class SandwichRecipeData : ScriptableObject
{
	public string recipeName;
	public Sprite icon;
	public List<IngredientData> ingredients;

	public bool CheckIngredientsAreCorrect(List<IngredientData> sandwichIngredients)
	{
		HashSet<IngredientData> recipeIngredientsSet = new HashSet<IngredientData>(ingredients);
		HashSet<IngredientData> sandwichIngredientsSet = new HashSet<IngredientData>(sandwichIngredients);

		return recipeIngredientsSet.SetEquals(sandwichIngredientsSet);
	}

	public bool CheckRecipeHasIngredient(IngredientData ingredient)
    {
		return ingredients.Contains(ingredient);
    }
}
