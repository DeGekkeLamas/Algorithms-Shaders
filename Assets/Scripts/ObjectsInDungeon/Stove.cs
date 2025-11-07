using InventoryStuff;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stove : MonoBehaviour, IInteractible
{
    public Recipe[] knownRecipes;
    public Vector2 dimensions;
    public Transform RecipeUI;
    public RawImage image;
    public ItemCrafter recipeDisplay;

    List<RawImage> spawnedObjects = new();

    public void OnInteract()
    {
        OpenMenu();
    }

    void OpenMenu()
    {
        RecipeUI.gameObject.SetActive(true);
        GenerateUI();
    }

    void GenerateUI()
    {
        int sqrtLength = Mathf.CeilToInt(Mathf.Sqrt(knownRecipes.Length));
        Vector2 sizePerRecipe = dimensions / sqrtLength;

        for(int y  = 0; y < sqrtLength; y++)
        {
            for(int x = 0; x < sqrtLength; x++)
            {
                int i = y * sqrtLength + x;
                if (i >= knownRecipes.Length) break;
                Recipe recipe = knownRecipes[i];
                InventoryItem recipeItem = recipe.result.GetItem();

                // Container
                Vector2 pos = new(sizePerRecipe.x * ((x) % sqrtLength),
                    sizePerRecipe.y * (y) / sqrtLength);
                ItemCrafter crafter = Instantiate(recipeDisplay, pos, Quaternion.identity, RecipeUI);
                crafter.recipe = recipe;
                // Items
                for (int j = 0; j < recipe.ingredients.Length; j++)
                {

                }
            }
        }

        //for (int i = 0; i < knownRecipes.Length; i++)
        //{

            //Vector2 recipeSize = sizePerRecipe / (recipe.ingredients.Length + 1);

                //SpawnImage(new Vector2(recipeSize.x * (i % sqrtLength), recipeSize.y * i / sqrtLength),
                //    Inventory.instance.Contains(recipeItem) ? recipeItem.ItemSprite : recipeItem.ItemSilhouette, recipeItem.itemName);
                //for (int j = 0; j < recipe.ingredients.Length; j++)
                //{
                //    InventoryItem current = recipe.ingredients[j].GetItem();
                //    Vector2 position = new(sizePerRecipe.x * ((j + 1) % sqrtLength),
                //        sizePerRecipe.y * (i + 1) / sqrtLength);
                //    SpawnImage(position, Inventory.instance.Contains(current) ? current.ItemSprite : current.ItemSilhouette, current.itemName);
                //}
        //}
    }

    void SpawnImage(Vector2 position, Texture2D texture, string name)
    {
        position *= RecipeUI.localScale;
        RawImage spawned = Instantiate(image, position, Quaternion.identity, RecipeUI);
        spawnedObjects.Add(spawned);
        spawned.texture = texture;
        spawned.name = name;
    }

    public void CloseMenu()
    {
        RecipeUI.gameObject.SetActive(false);
    }
}
