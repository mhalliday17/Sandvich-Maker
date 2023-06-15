using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    public UIMainMenuController uIMainMenuController;
    public UIMatchEndMenu uIMatchEndMenu;
    public MatchController matchController;
    public UIIngredientsSelection uIIngredientsSelection;
    public SandwichBuilderController sandwichBuilderController;

    [SerializeField] private List<SandwichRecipeData> sandwichRecipeDataList;
    [SerializeField] private List<IngredientData> ingredientDataList;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public List<SandwichRecipeData> GetRandomizedSandwichRecipeDataList()
    {
        List<SandwichRecipeData> randomSandwichRecipeDataList = new List<SandwichRecipeData>(sandwichRecipeDataList);

        int n = randomSandwichRecipeDataList.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            SandwichRecipeData temp = randomSandwichRecipeDataList[k];
            randomSandwichRecipeDataList[k] = randomSandwichRecipeDataList[n];
            randomSandwichRecipeDataList[n] = temp;
        }

        return randomSandwichRecipeDataList;
    }

    public SandwichRecipeData GetRandomRecipe()
    {
        return sandwichRecipeDataList[Random.Range(0, sandwichRecipeDataList.Count)];
    }

    public IngredientData GetIngredientDataById(int ingredientId)
    {
        foreach (var item in ingredientDataList)
        {
            if (item.id == ingredientId) return item;
        }
        return null;
    }
}
