using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIngredientsSelection : MonoBehaviour
{
    [SerializeField] private GameObject buttonsContent;
    [SerializeField] private Button bolognaButton;
    [SerializeField] private Button cheeseButton;
    [SerializeField] private Button hamButton;
    [SerializeField] private Button lettuceButton;
    [SerializeField] private Button tomatoButton;

    public event Action<int> IngredientSelected;

    private void Start()
    {
        LoadSelectionButtonsActions();
        buttonsContent.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (buttonsContent != null)
        {
            GameManager.Instance.matchController.MatchOnCourseOnChange += StartRecipeUI;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.matchController.MatchOnCourseOnChange -= StartRecipeUI;
    }

    private void StartRecipeUI(bool matchOnCourse)
    {
        buttonsContent.gameObject.SetActive(matchOnCourse);
    }

    private void LoadSelectionButtonsActions()
    {
        bolognaButton.onClick.AddListener(() => SelectIngredient(0));
        cheeseButton.onClick.AddListener(() => SelectIngredient(1));
        hamButton.onClick.AddListener(() => SelectIngredient(2));
        lettuceButton.onClick.AddListener(() => SelectIngredient(3));
        tomatoButton.onClick.AddListener(() => SelectIngredient(4));
    }

    private void SelectIngredient(int ingredientID)
    {
        IngredientSelected?.Invoke(ingredientID);
    }
}
