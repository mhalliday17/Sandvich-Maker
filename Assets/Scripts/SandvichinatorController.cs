using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SandvichinatorController : MonoBehaviour
{
    private int ingredientsCount;
    
    [SerializeField] private GameObject sandvichContent;

    [SerializeField] private float ingredientSpacingValue;

    [SerializeField] private GameObject breadUp;
    [SerializeField] private List<GameObject> ingredientsList;

    [SerializeField] private PlayableDirector animDirector;

    private List<Action> queuedActions = new List<Action>();

    private void OnEnable()
    {
        GameManager.Instance.sandwichBuilderController.SandwichRecipeLoaded += (recipe) => { LoadActionToQueue(() => LoadNewSandvich()); };
        GameManager.Instance.sandwichBuilderController.SandwichIngredientAdded += (ingredient) => { LoadActionToQueue(() => AddIngredient(ingredient)); };

        animDirector.stopped += (a) => { CheckQueuedActions(); };
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.sandwichBuilderController.SandwichRecipeLoaded -= (recipe) => { LoadActionToQueue(() => LoadNewSandvich()); };
        GameManager.Instance.sandwichBuilderController.SandwichIngredientAdded -= (ingredient) => { LoadActionToQueue(() => AddIngredient(ingredient)); };

        animDirector.stopped -= (a) => { CheckQueuedActions(); };
    }

    private void CheckQueuedActions()
    {
        if (queuedActions.Count > 0)
            TriggerShutter();
    }

    private void TriggerShutter()
    {
        animDirector.Play();
    }

    private void LoadActionToQueue(Action action)
    {
        queuedActions.Add(action);
        CheckQueuedActions();
    }

    public void OnShutterClosed()
    {
        if(queuedActions.Count > 0)
        {
            queuedActions[0].Invoke();
            queuedActions.RemoveAt(0);
        }
    }

    private void LoadNewSandvich()
    {
        ingredientsCount = 0;
        breadUp.SetActive(false);
        DisableAllIngredients();
    }

    private void LoadFinishSandvich()
    {
        breadUp.SetActive(true);
    }

    private void AddIngredient(IngredientData ingredient)
    {
        for (int i = 0; i < ingredientsList.Count; i++)
        {
            if(i == ingredient.id)
            {
                ingredientsList[i].gameObject.transform.localPosition = new Vector3(0f, ingredientsCount * ingredientSpacingValue, 0f);
                ingredientsList[i].SetActive(true);
                ingredientsCount++;
            }
        }
    }

    private void DisableAllIngredients()
    {
        foreach (var item in ingredientsList)
        {
            item.SetActive(false);
        }
    }
}
