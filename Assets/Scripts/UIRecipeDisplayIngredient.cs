using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRecipeDisplayIngredient : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Image border;
    private IngredientData currentIngredient;

    private void OnEnable()
    {
        GameManager.Instance.sandwichBuilderController.SandwichIngredientAdded += IsSelected;
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.sandwichBuilderController.SandwichIngredientAdded -= IsSelected;
    }

    public void LoadIngredient(IngredientData ingredient)
    {
        currentIngredient = ingredient;
        border.color = currentIngredient.color;
        icon.sprite = currentIngredient.icon;
    }

    private void IsSelected(IngredientData ingredient)
    {
        if(currentIngredient == ingredient)
        {
            border.color = Color.black;
        }
    }
}
