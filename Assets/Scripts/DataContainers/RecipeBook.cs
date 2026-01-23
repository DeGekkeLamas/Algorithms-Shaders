using UnityEngine;


namespace InventoryStuff
{
    /// <summary>
    /// Collection of all known recipes
    /// </summary>
    [CreateAssetMenu(fileName = "RecipeBook", menuName = "ScriptableObjects/RecipeBook")]
    public class RecipeBook : ScriptableObject
    {
        public Recipe[] recipes;
    }
}
