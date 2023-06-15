using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandwichBuilderController : MonoBehaviour
{
    private SandwichRecipeData currentRecipe;
    private Sandwich currentSandwich;

    public event Action<SandwichRecipeData> SandwichRecipeLoaded;
    public event Action<IngredientData> SandwichIngredientAdded;
    public event Action<bool> SandwichFinished;

    private void OnEnable()
    {
        GameManager.Instance.matchController.MatchOnCourseOnChange += (matchOnCourse) => { if (matchOnCourse) StartNewSandwichBuild(); else currentSandwich = null; };
        GameManager.Instance.uIIngredientsSelection.IngredientSelected += (ingredientId) => { AddSelectedIngredient(ingredientId); };
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null)
            return;
        GameManager.Instance.matchController.MatchOnCourseOnChange -= (matchOnCourse) => { if (matchOnCourse) StartNewSandwichBuild(); else currentSandwich = null; };
        GameManager.Instance.uIIngredientsSelection.IngredientSelected -= (ingredientId) => { AddSelectedIngredient(ingredientId); };
    }

    private void StartNewSandwichBuild()
    {
        currentRecipe = GameManager.Instance.GetRandomRecipe();
        if (currentSandwich == null) currentSandwich = gameObject.AddComponent<Sandwich>();
        currentSandwich.Initialize();
        SandwichRecipeLoaded?.Invoke(currentRecipe);
    }

    private void AddSelectedIngredient(int ingredientId)
    {
        if(currentSandwich.currentIngredients.Count < currentRecipe.ingredients.Count)
        {
            IngredientData ingredient = GameManager.Instance.GetIngredientDataById(ingredientId);
            if (!currentRecipe.CheckRecipeHasIngredient(ingredient) || currentSandwich.CheckIngredientIsDuplicated(ingredient))
            {
                SandwichFinished?.Invoke(false);
                StartNewSandwichBuild();
                return;
            }

            currentSandwich.AddIngredient(ingredient);
            SandwichIngredientAdded?.Invoke(ingredient);

            if(currentSandwich.currentIngredients.Count == currentRecipe.ingredients.Count)
            {
                SandwichFinished?.Invoke(true);
                StartNewSandwichBuild();
            }
        }
    }
}

public class Sandwich : MonoBehaviour
{
    public List<IngredientData> currentIngredients = new List<IngredientData>();

    public void Initialize()
    {
        currentIngredients = new List<IngredientData>();
    }

    public void AddIngredient(IngredientData ingredient)
    {
        currentIngredients.Add(ingredient);
    }

    public bool CheckIngredientIsDuplicated(IngredientData ingredient)
    {
        return currentIngredients.Contains(ingredient);
    }
}
