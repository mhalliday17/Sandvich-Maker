using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewIngredientData", menuName ="SandwichData/IngredientData")]
public class IngredientData : ScriptableObject
{
    public int id;
    public string ingredientName;
    public Color color;
    public Sprite icon;
}
