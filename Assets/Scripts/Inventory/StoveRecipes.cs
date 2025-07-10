using UnityEngine;

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
            stoveRecipes[i].name = stoveRecipes[i].result.name;
        }
    }
}

[System.Serializable]
public struct Recipe
{
    [Tooltip("This variable is for clarity and not meant to be modified")]
    public string name;

    public InventoryItem result;
    public InventoryItem[] ingredients;
}
