using UnityEngine;

public class ItemCheats : MonoBehaviour
{
    Inventory inventory;
    ItemPresets presets;
    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        presets = GetComponent<ItemPresets>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Equals))
        {
            switch(Input.inputString)
            {
                case "q":
                    inventory.AddItem(presets.apron);
                    break;
                case "w":
                    inventory.AddItem(presets.banana);
                    break;
                case "e":
                    inventory.AddItem(presets.bread);
                    break;
                case "r":
                    inventory.AddItem(presets.butter);
                    break;
                case "t":
                    inventory.AddItem(presets.cake);
                    break;
                case "y":
                    inventory.AddItem(presets.cakeKnife);
                    break;
                case "u":
                    inventory.AddItem(presets.cheese);
                    break;
                case "i":
                    inventory.AddItem(presets.cheesePizza);
                    break;
                case "o":
                    inventory.AddItem(presets.chefHat);
                    break;
                case "p":
                    inventory.AddItem(presets.cryingPan);
                    break;
                case "a":
                    inventory.AddItem(presets.cuttingBoard);
                    break;
                case "s":
                    inventory.AddItem(presets.dryingPan);
                    break;
                case "d":
                    inventory.AddItem(presets.dyingPan);
                    break;
                case "f":
                    inventory.AddItem(presets.egg);
                    break;
                case "g":
                    inventory.AddItem(presets.emptyMicrowave);
                    break;
                case "h":
                    inventory.AddItem(presets.fork);
                    break;
                case "j":
                    inventory.AddItem(presets.fryingPan);
                    break;
                case "k":
                    inventory.AddItem(presets.grilledCheeseSandwich);
                    break;
                case "l":
                    inventory.AddItem(presets.grilledChicken);
                    break;
                case "z":
                    inventory.AddItem(presets.identifyingPan);
                    break;
                case "x":
                    inventory.AddItem(presets.jalapeno);
                    break;
                case "c":
                    inventory.AddItem(presets.ketchupBottle);
                    break;
                case "v":
                    inventory.AddItem(presets.knife);
                    break;
                case "bb":
                    inventory.AddItem(presets.knork);
                    break;
                case "n":
                    inventory.AddItem(presets.loadedMicrowave);
                    break;
                case "m":
                    inventory.AddItem(presets.mystifyingPan);
                    break;
                case "Q":
                    inventory.AddItem(presets.pepperShaker);
                    break;
                case "W":
                    inventory.AddItem(presets.pipingBag);
                    break;
                case "E":
                    inventory.AddItem(presets.pizza);
                    break;
                case "R":
                    inventory.AddItem(presets.plate);
                    break;
                case "T":
                    inventory.AddItem(presets.rawChicken);
                    break;
                case "Y":
                    inventory.AddItem(presets.rollingPin);
                    break;
                case "U":
                    inventory.AddItem(presets.saltShaker);
                    break;
                case "I":
                    inventory.AddItem(presets.spicyPizza);
                    break;
                case "O":
                    inventory.AddItem(presets.spife);
                    break;
                case "P":
                    inventory.AddItem(presets.spoon);
                    break;
                case "A":
                    inventory.AddItem(presets.sporf);
                    break;
                case "S":
                    inventory.AddItem(presets.spork);
                    break;
                case "D":
                    inventory.AddItem(presets.stainlessSteelPot);
                    break;
                case "F":
                    inventory.AddItem(presets.sausage);
                    break;
                case "G":
                    inventory.AddItem(presets.toaster);
                    break;
                case "H":
                    inventory.AddItem(presets.tomato);
                    break;
                case "J":
                    inventory.AddItem(presets.tomatoPizza);
                    break;
                case "K":
                    inventory.AddItem(presets.towel);
                    break;
                case "L":
                    inventory.AddItem(presets.waterBottle);
                    break;
                case "1":
                    inventory.AddItem(presets.blessedCheese);
                    break;
                case "2":
                    inventory.AddItem(presets.undyingPan);
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
