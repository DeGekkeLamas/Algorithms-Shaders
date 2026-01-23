using InventoryStuff;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryStuff
{
    /// <summary>
    /// Manages the crafting UI when the stove object is interacted with. I hate this script and working with UI in code
    /// </summary>
    public class Stove : MonoBehaviour, IInteractible
    {
        [SerializeField] RecipeBook knownRecipes;
        [SerializeField] RectInt dimensions;
        Transform RecipeUI;
        [SerializeField] RawImage image;
        [SerializeField] ItemCrafter recipeDisplay;

        static List<GameObject> spawnedObjects = new();
        static Dictionary<RawImage, InventoryItem> imageTextures = new();
        bool isGenerated;
        public static bool menuIsOpen;

        private void Start()
        {
            RecipeUI = GameManager.instance.RecipeUI;
        }

        private void Update()
        {
            if (menuIsOpen && Input.GetKeyDown(KeyCode.Escape)) CloseMenu();
        }


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

        public static void CloseMenu()
        {
            menuIsOpen = false;
            DisableAll();
            GameManager.instance.RecipeUI.gameObject.SetActive(false);
        }

        void GenerateUI()
        {
            int sqrtLength = Mathf.CeilToInt(Mathf.Sqrt(knownRecipes.recipes.Length));
            Vector2 sizePerRecipe = (dimensions.max - dimensions.min) / sqrtLength;

            for (int y = 0; y < sqrtLength; y++)
            {
                for (int x = 0; x < sqrtLength; x++)
                {
                    int i = y * sqrtLength + x;
                    if (i >= knownRecipes.recipes.Length) break;
                    Recipe recipe = knownRecipes.recipes[i];
                    InventoryItem recipeItem = recipe.result.GetItem();

                    // Container
                    Vector3 pos = new Vector2(sizePerRecipe.x * (x % sqrtLength),
                        sizePerRecipe.y * y / sqrtLength) + dimensions.min;
                    ItemCrafter crafter = Instantiate(recipeDisplay, pos * RecipeUI.transform.localScale.x, Quaternion.identity, RecipeUI);
                    crafter.recipe = recipe;
                    spawnedObjects.Add(crafter.gameObject);

                    /// Items
                    // Result
                    RawImage resultImage = SpawnImage(pos + new Vector3(0, sizePerRecipe.y * .025f)
                        , Inventory.instance.Contains(recipeItem) ? SpriteEditor.RemoveBG(recipeItem.ItemSprite) :
                        recipeItem.ItemSilhouette, recipeItem.itemName);
                    imageTextures[resultImage] = recipeItem;
                    // Ingredients
                    for (int j = 0; j < recipe.ingredients.Length; j++)
                    {
                        InventoryItem currentItem = recipe.ingredients[j].GetItem();
                        Vector2 position = pos + new Vector3(Mathf.Lerp(-sizePerRecipe.x, sizePerRecipe.x,
                            Mathf.InverseLerp(0, recipe.ingredients.Length - 1, j)) * .25f, -sizePerRecipe.y * .025f);
                        RawImage ingredientImage = SpawnImage(position, Inventory.instance.Contains(currentItem) ?
                            SpriteEditor.RemoveBG(currentItem.ItemSprite) : currentItem.ItemSilhouette, currentItem.itemName);
                        imageTextures[ingredientImage] = currentItem;

                    }
                }
            }
            isGenerated = true;
        }

        RawImage SpawnImage(Vector2 position, Texture2D texture, string name)
        {
            position *= RecipeUI.localScale;
            RawImage spawned = Instantiate(image, position, Quaternion.identity, RecipeUI);
            spawnedObjects.Add(spawned.gameObject);
            spawned.texture = texture;
            spawned.name = name;
            return spawned;
        }

        public static void EnableAll()
        {
            for (int i = spawnedObjects.Count - 1; i > 0; i--)
            {
                GameObject obj = spawnedObjects[i];
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
            foreach (KeyValuePair<RawImage, InventoryItem> pair in imageTextures)
            {
                if (pair.Value != null)
                {
                    pair.Key.texture = Inventory.instance.Contains(pair.Value) ? SpriteEditor.RemoveBG(pair.Value.ItemSprite) :
                            pair.Value.ItemSilhouette;
                }
            }
        }

        public static void DisableAll()
        {
            for (int i = spawnedObjects.Count - 1; i > 0; i--)
            {
                GameObject obj = spawnedObjects[i];
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
