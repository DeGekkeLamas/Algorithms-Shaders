using UnityEngine;
using InventoryStuff;

public class StoveRecipes : MonoBehaviour
{
    public Recipe[] stoveRecipes;
    public static Recipe[] recipes;

    private void Awake()
    {
        recipes = stoveRecipes;
    }

    private void OnValidate()
    {
        for (int i = 0; i < stoveRecipes.Length; i++)
        {
            //stoveRecipes[i].name = stoveRecipes[i].result.name;
        }
    }
}

