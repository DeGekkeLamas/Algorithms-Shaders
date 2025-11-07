using InventoryStuff;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Stove : MonoBehaviour, IInteractible
{
    public Recipe[] knownRecipes;
    public RectInt dimensions;
    public Transform RecipeUI;
    public RawImage image;
    public ItemCrafter recipeDisplay;

    static List<GameObject> spawnedObjects = new();
    bool isGenerated;
    static bool menuIsOpen;

    public void OnInteract()
    {
        if (menuIsOpen) CloseMenu();
        else OpenMenu();
    }

    void OpenMenu()
    {
        menuIsOpen = true;
        RecipeUI.gameObject.SetActive(true);
        if (isGenerated) EnableAll();
        else GenerateUI();
    }

    public void CloseMenu()
    {
        menuIsOpen = false;
        DisableAll();
        RecipeUI.gameObject.SetActive(false);
    }

    void GenerateUI()
    {
        int sqrtLength = Mathf.CeilToInt(Mathf.Sqrt(knownRecipes.Length));
        Vector2 sizePerRecipe = (dimensions.max - dimensions.min) / sqrtLength;

        for(int y  = 0; y < sqrtLength; y++)
        {
            for(int x = 0; x < sqrtLength; x++)
            {
                int i = y * sqrtLength + x;
                if (i >= knownRecipes.Length) break;
                Recipe recipe = knownRecipes[i];
                InventoryItem recipeItem = recipe.result.GetItem();

                // Container
                Vector3 pos = new Vector2(sizePerRecipe.x * (x % sqrtLength),
                    sizePerRecipe.y * y / sqrtLength) + dimensions.min;
                ItemCrafter crafter = Instantiate(recipeDisplay, pos * RecipeUI.transform.localScale.x, Quaternion.identity, RecipeUI);
                crafter.recipe = recipe;
                spawnedObjects.Add(crafter.gameObject);

                /// Items
                // Result
                SpawnImage(pos + new Vector3(0, sizePerRecipe.y * .025f)
                    , Inventory.instance.Contains(recipeItem) ? SpriteEditor.RemoveBG(recipeItem.ItemSprite) : 
                    recipeItem.ItemSilhouette, recipeItem.itemName);
                // Ingredients
                for (int j = 0; j < recipe.ingredients.Length; j++)
                {
                    InventoryItem currentItem = recipe.ingredients[j].GetItem();
                    Vector2 position = pos + 
                        new Vector3(Mathf.Lerp(-sizePerRecipe.x, sizePerRecipe.x, 1f/(Mathf.Min(1,recipe.ingredients.Length-1)) * (j)) * .25f , -sizePerRecipe.y * .025f);
                    SpawnImage(position, Inventory.instance.Contains(currentItem) ?
                        SpriteEditor.RemoveBG(currentItem.ItemSprite) : currentItem.ItemSilhouette, currentItem.itemName);

                }
            }
        }
        isGenerated = true;
    }

    void SpawnImage(Vector2 position, Texture2D texture, string name)
    {
        position *= RecipeUI.localScale;
        RawImage spawned = Instantiate(image, position, Quaternion.identity, RecipeUI);
        spawnedObjects.Add(spawned.gameObject);
        spawned.texture = texture;
        spawned.name = name;
    }

    public static void EnableAll()
    {
        for(int i = spawnedObjects.Count-1; i > 0; i--)
        {
            spawnedObjects[i].SetActive(true);
        }
    }

    public static void DisableAll()
    {
        for(int i = spawnedObjects.Count-1; i > 0; i--)
        {
            spawnedObjects[i].SetActive(false);
        }
    }
}
