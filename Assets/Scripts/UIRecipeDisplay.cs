using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRecipeDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeName;
    [SerializeField] private Image recipeIcon;
    [SerializeField] private GameObject recipeContent;
    [SerializeField] private GameObject ingredientsIconsContent;
    [SerializeField] private UIRecipeDisplayIngredient ingredientIconPrefab;

    [SerializeField] private List<UIRecipeDisplayIngredient> uIRecipeDisplayIngredientList = new List<UIRecipeDisplayIngredient>();

    private void Start()
    {
        recipeContent.gameObject.SetActive(false);
        DestroyIngredientIcon(ingredientsIconsContent.transform.childCount);
    }

    private void OnEnable()
    {
        if (ingredientsIconsContent != null)
        {
            GameManager.Instance.matchController.MatchOnCourseOnChange += StartRecipeUI;
            GameManager.Instance.sandwichBuilderController.SandwichRecipeLoaded += LoadRecipeUI;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.matchController.MatchOnCourseOnChange -= StartRecipeUI;
        GameManager.Instance.sandwichBuilderController.SandwichRecipeLoaded -= LoadRecipeUI;
    }

    private void StartRecipeUI(bool matchOnCourse)
    {
        recipeContent.gameObject.SetActive(matchOnCourse);
    }

    private void LoadRecipeUI(SandwichRecipeData recipeData)
    {
        recipeName.text = recipeData.recipeName;
        recipeIcon.sprite = recipeData.icon;

        int dif = recipeData.ingredients.Count - ingredientsIconsContent.transform.childCount;

        if (dif > 0)
            CreateIngredientIcon(dif);
        else if(dif < 0)
            DestroyIngredientIcon(dif);

        for (int i = 0; i < uIRecipeDisplayIngredientList.Count; i++)
        {
            uIRecipeDisplayIngredientList[i].LoadIngredient(recipeData.ingredients[i]);
        }
    }

    private void CreateIngredientIcon(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            UIRecipeDisplayIngredient ingredientIcon = Instantiate(ingredientIconPrefab, ingredientsIconsContent.transform);
            uIRecipeDisplayIngredientList.Add(ingredientIcon);
        }
    }

    private void DestroyIngredientIcon(int quantity)
    {
        for (int i = 0; i < Mathf.Abs(quantity); i++)
        {
            Destroy(uIRecipeDisplayIngredientList[^1].gameObject);
            uIRecipeDisplayIngredientList.RemoveAt(uIRecipeDisplayIngredientList.Count - 1);
        }
    }
}