using UnityEngine;

public class ItemCheats : MonoBehaviour
{
    Inventory inventory;
    private void Awake() => inventory = GetComponent<Inventory>();
    private void Update()
    {
        if (Input.GetKey(KeyCode.Equals))
        {
            switch(Input.inputString)
            {
                case "q":
                    inventory.AddItem(ItemPresets.presets["Apron"]);
                    break;
                case "w":
                    inventory.AddItem(ItemPresets.presets["Banana"]);
                    break;
                case "e":
                    inventory.AddItem(ItemPresets.presets["Bread"]);
                    break;
                case "r":
                    inventory.AddItem(ItemPresets.presets["Butter"]);
                    break;
                case "t":
                    inventory.AddItem(ItemPresets.presets["Cake"]);
                    break;
                case "y":
                    inventory.AddItem(ItemPresets.presets["CakeKnife"]);
                    break;
                case "u":
                    inventory.AddItem(ItemPresets.presets["Cheese"]);
                    break;
                case "i":
                    inventory.AddItem(ItemPresets.presets["CheesePizza"]);
                    break;
                case "o":
                    inventory.AddItem(ItemPresets.presets["ChefHat"]);
                    break;
                case "p":
                    inventory.AddItem(ItemPresets.presets["CryingPan"]);
                    break;
                case "a":
                    inventory.AddItem(ItemPresets.presets["CuttingBoard"]);
                    break;
                case "s":
                    inventory.AddItem(ItemPresets.presets["DryingPan"]);
                    break;
                case "d":
                    inventory.AddItem(ItemPresets.presets["DyingPan"]);
                    break;
                case "f":
                    inventory.AddItem(ItemPresets.presets["Egg"]);
                    break;
                case "g":
                    inventory.AddItem(ItemPresets.presets["EmptyMicrowave"]);
                    break;
                case "h":
                    inventory.AddItem(ItemPresets.presets["Fork"]);
                    break;
                case "j":
                    inventory.AddItem(ItemPresets.presets["FryingPan"]);
                    break;
                case "k":
                    inventory.AddItem(ItemPresets.presets["GrilledCheeseSandwich"]);
                    break;
                case "l":
                    inventory.AddItem(ItemPresets.presets["GrilledChicken"]);
                    break;
                case "z":
                    inventory.AddItem(ItemPresets.presets["IdentifyingPan"]);
                    break;
                case "x":
                    inventory.AddItem(ItemPresets.presets["Jalapeno"]);
                    break;
                case "c":
                    inventory.AddItem(ItemPresets.presets["KetchupBottle"]);
                    break;
                case "v":
                    inventory.AddItem(ItemPresets.presets["Knife"]);
                    break;
                case "bb":
                    inventory.AddItem(ItemPresets.presets["Knork"]);
                    break;
                case "n":
                    inventory.AddItem(ItemPresets.presets["LoadedMicrowave"]);
                    break;
                case "m":
                    inventory.AddItem(ItemPresets.presets["MystifyingPan"]);
                    break;
                case "Q":
                    inventory.AddItem(ItemPresets.presets["PepperShaker"]);
                    break;
                case "W":
                    inventory.AddItem(ItemPresets.presets["PipingBag"]);
                    break;
                case "E":
                    inventory.AddItem(ItemPresets.presets["Pizza"]);
                    break;
                case "R":
                    inventory.AddItem(ItemPresets.presets["Plate"]);
                    break;
                case "T":
                    inventory.AddItem(ItemPresets.presets["RawChicken"]);
                    break;
                case "Y":
                    inventory.AddItem(ItemPresets.presets["RollingPin"]);
                    break;
                case "U":
                    inventory.AddItem(ItemPresets.presets["SaltShaker"]);
                    break;
                case "I":
                    inventory.AddItem(ItemPresets.presets["SpicyPizza"]);
                    break;
                case "O":
                    inventory.AddItem(ItemPresets.presets["Spife"]);
                    break;
                case "P":
                    inventory.AddItem(ItemPresets.presets["Spoon"]);
                    break;
                case "A":
                    inventory.AddItem(ItemPresets.presets["Sporf"]);
                    break;
                case "S":
                    inventory.AddItem(ItemPresets.presets["Spork"]);
                    break;
                case "D":
                    inventory.AddItem(ItemPresets.presets["StainlessSteelPot"]);
                    break;
                case "F":
                    inventory.AddItem(ItemPresets.presets["Sausage"]);
                    break;
                case "G":
                    inventory.AddItem(ItemPresets.presets["Toaster"]);
                    break;
                case "H":
                    inventory.AddItem(ItemPresets.presets["Tomato"]);
                    break;
                case "J":
                    inventory.AddItem(ItemPresets.presets["TomatoPizza"]);
                    break;
                case "K":
                    inventory.AddItem(ItemPresets.presets["Towel"]);
                    break;
                case "L":
                    inventory.AddItem(ItemPresets.presets["WaterBottle"]);
                    break;
                case "Z":
                    inventory.AddItem(ItemPresets.presets["Baguette"]);
                    break;
                case "1":
                    inventory.AddItem(ItemPresets.presets["BlessedCheese"]);
                    break;
                case "2":
                    inventory.AddItem(ItemPresets.presets["UndyingPan"]);
                    break;

                case "-":
                    for(int i = 0; i < inventory.currentInventory.Length; i++)
                        inventory.RemoveItem(i);
                    Debug.Log($"Cleared inventory, from {this}");
                    break;
            }
        }
    }
}
