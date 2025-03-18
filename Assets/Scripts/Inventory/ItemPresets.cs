using System.Collections.Generic;
using UnityEngine;

public class ItemPresets : MonoBehaviour
{
    public static Dictionary<string, InventoryItem> presets = new Dictionary<string, InventoryItem>();
    public GameObject[] itemModels;
    public GameObject placeholderModel;
    public Rigidbody[] projectiles;
    public Rigidbody placeholderProjectile;
    private void Awake()
    {
        presets["Apron"] = new InventoryItem
        {
            itemName = "Apron",
            foodResistanceBoost = true,
            toolTip = "What does your grandma's death have to do with the blo- tomato stains on this?"
        };
        presets["Baguette"] = new InventoryItem
        {
            itemName = "Baguette",
            isFood = true,
            damage = 45,
            durability = 10,
            toolTip = "A French katana, or a French Japanese sword.",
            canAttack = true,
        };
        presets["Banana"] = new InventoryItem
        {
            itemName = "Banana",
            isConsumedOnUse = true,
            damage = 80,
            hpHealed = 40,
            canAttack = true,
            canConsume = true,
            isFood = true,
            toolTip = "Will you use this as gun or as a phone? Will you pick the dark side or the light side?",
            hasOverworldUses = true,
        };
        presets["Bread"] = new InventoryItem
        {
            itemName = "Bread",
            isStackable = true,
            maxStack = 5,
            toolTip = "Hansel and Gretel GPS.",
            isStoveIngredient = true,
        };
        presets["Butter"] = new InventoryItem
        {
            itemName = "Butter",
            damage = 10,
            isConsumedOnUse = true,
            inflictStuck = true,
            toolTip = "It's like sunscreen but better!",
            canThrow = true,
            hasOverworldUses = true,
        };
        presets["Cake"] = new InventoryItem
        {
            itemName = "Cake",
            isConsumedOnUse = true,
            damage = 25,
            inflictBlindness = true,
            isFood = true,
            toolTip = "Just as caked out as your mom.",
            canThrow = true,
            hasOverworldUses = true,
        };
        presets["CakeKnife"] = new InventoryItem
        {
            itemName = "Cake knife",
            damage = 60,
            durability = 20,
            isMetal = true,
            isKnife = true,
            toolTip = "Great for stabbing cakes!",
            canAttack = true,
            isStoveIngredient = true,
        };
        presets["Cheese"] = new InventoryItem
        {
            itemName = "Cheese",
            isConsumedOnUse = true,
            hpHealed = 40,
            toolTip = "Say cheese when taking a picture",
            canConsume = true,
            isStoveIngredient = true,
            hasOverworldUses = true,
        };
        presets["CheesePizza"] = new InventoryItem
        {
            itemName = "Cheese pizza",
            damage = 25,
            durability = 10,
            amountOfHits = 2,
            onAttackHealHP = true,
            isFood = true,
            toolTip = "You can never have enough cheese.",
            canAttack = true,
            hasOverworldUses = true,
        };
        presets["ChefHat"] = new InventoryItem
        {
            itemName = "Chef hat",
            foodBoost = true,
            toolTip = ""
        };
        presets["CryingPan"] = new InventoryItem
        {
            itemName = "Crying pan",
            damage = 60,
            durability = 30,
            canAttack = true,
            canBlock = true,
            fireImmunity = true,
            isMetal = true,
            toolTip = "Why the long handle?",
            isStoveIngredient = true,
        };
        presets["CuttingBoard"] = new InventoryItem
        {
            itemName = "Cutting board",
            durability = 50,
            knifeBoost = true,
            toolTip = "",
            canBlock = true
        };
        presets["DryingPan"] = new InventoryItem
        {
            itemName = "Drying pan",
            damage = 60,
            durability = 30,
            canAttack = true,
            canBlock = true,
            durabilityBoost = true,
            isMetal = true,
            toolTip = "I'm gonna use my frying pan as a drying pan!",
            isStoveIngredient = true,
        };
        presets["DyingPan"] = new InventoryItem
        {
            itemName = "Dying pan",
            damage = 80,
            durability = 30,
            canAttack = true,
            canBlock = true,
            isKnife = true,
            isMetal = true,
            toolTip = "Not to be confused with a dyeing pan.",
            isStoveIngredient = true,
        };
        presets["Egg"] = new InventoryItem
        {
            itemName = "Egg",
            isConsumedOnUse = true,
            isStackable = true,
            maxStack = 10,
            damage = 40,
            isFood = true,
            toolTip = "One of the most popular forms of child to eat",
            canThrow = true,
            hasOverworldUses = true,
        };
        presets["EmptyMicrowave"] = new InventoryItem
        {
            itemName = "Empty microwave",
            isEmptyMicrowave = true,
            toolTip = "",
        };
        presets["Fork"] = new InventoryItem
        {
            itemName = "Fork",
            damage = 25,
            durability = 10,
            canAttack = true,
            healingBoost = 10,
            isMetal = true,
            toolTip = "What the fork",
            isStoveIngredient = true,
        };
        presets["FryingPan"] = new InventoryItem
        {
            itemName = "Frying pan",
            damage = 50,
            durability = 20,
            canAttack = true,
            canBlock = true,
            isMetal = true,
            toolTip = "You're gonna cook!",
            isStoveIngredient = true,
        };
        presets["GrilledCheeseSandwich"] = new InventoryItem
        {
            itemName = "Grilled cheese sandwich",
            isConsumedOnUse = true,
            hpHealed = 70,
            toolTip = "Nice and warm like a witch in the sand",
            canConsume = true,
            hasOverworldUses = true,
        };
        presets["GrilledChicken"] = new InventoryItem
        {
            itemName = "Grilled chicken",
            hpHealed = 60,
            toolTip = "",
            canConsume = true,
            hasOverworldUses = true,
        };
        presets["IdentifyingPan"] = new InventoryItem
        {
            itemName = "Identifying pan",
            damage = 60,
            durability = 30,
            canAttack = true,
            canBlock = true,
            seeEnemyInventories = true,
            isMetal = true,
            toolTip = "Legend has it the identifying pan is also identifying as pan.",
            isStoveIngredient = true,
        };
        presets["Jalapeno"] = new InventoryItem
        {
            itemName = "Jalapeño",
            isConsumedOnUse = true,
            damage = 20,
            hpHealed = 50,
            inflictOnFire = true,
            canThrow = true,
            canConsume = true,
            isFood = true,
            toolTip = "An arsonists favorite fruit!",
            isStoveIngredient = true,
            hasOverworldUses = true,
        };
        presets["KetchupBottle"] = new InventoryItem
        {
            itemName = "Ketchup bottle",
            isConsumedOnUse = true,
            inflictFakeBlood = true,
            hpHealed = 30,
            damage = 30,
            toolTip = "Reminds you of the dentist...",
            canThrow = true,
            canConsume = true,
            hasOverworldUses = true,
        };
        presets["Knife"] = new InventoryItem
        {
            itemName = "Knife",
            damage = 30,
            durability = 15,
            canAttack = true,
            healingBoost = 10,
            isMetal = true,
            isKnife = true,
            toolTip = "Great for if you like eating sliced bread or sliced people!",
            isStoveIngredient = true,
        };
        presets["Knork"] = new InventoryItem
        {
            itemName = "Knork",
            damage = 45,
            durability = 20,
            canAttack = true,
            healingBoost = 20,
            isMetal = true,
            isKnife = true,
            toolTip = "",
            isStoveIngredient = true,
        };
        presets["LoadedMicrowave"] = new InventoryItem
        {
            itemName = "Loaded microwave",
            isConsumedOnUse = true,
            targetAll = true,
            damage = 80,
            isFood = true,
            toolTip = "This thing is the bomb!",
            canThrow = true,
            hasOverworldUses = true,
        };
        presets["MystifyingPan"] = new InventoryItem
        {
            itemName = "Mystifying pan",
            damage = 70,
            durability = 60,
            canAttack = true,
            canBlock = true,
            seeEnemyInventories = true,
            durabilityBoost = true,
            fireImmunity = true,
            isKnife = true,
            amountOfTargets = 2,
            isMetal = true,
            toolTip = "One pan to rule them all...",
            hasOverworldUses = true,
        };
        presets["PepperShaker"] = new InventoryItem
        {
            itemName = "Pepper shaker",
            isConsumedOnUse = true,
            inflictOnFire = true,
            targetAll = true,
            toolTip = "Tasty flavor ashes!",
            canThrow = true,
            hasOverworldUses = true,
        };
        presets["PipingBag"] = new InventoryItem
        {
            itemName = "Piping bag",
            damage = 25,
            amountOfTargets = 3,
            toolTip = "",
            canAttack = true,
            hasOverworldUses = true,
        };
        presets["Pizza"] = new InventoryItem
        {
            itemName = "Pizza",
            damage = 25,
            durability = 10,
            amountOfHits = 2,
            isFood = true,
            toolTip = "An italian frisbee",
            canAttack = true,
            isStoveIngredient = true,
            hasOverworldUses = true,
        };
        presets["Plate"] = new InventoryItem
        {
            itemName = "Plate",
            isConsumedOnUse = true,
            damage = 35,
            damageAfterBlock = 15,
            toolTip = "Good thing it doesn't fly back when thrown as frisbee!",
            canThrow = true,
            canBlock = true,
            hasOverworldUses = true,
        };
        presets["RawChicken"] = new InventoryItem
        {
            itemName = "Raw chicken",
            damage = 20,
            hpHealed = 10,
            inflictPoisoned = true,
            toolTip = "Alternate ending to Chicken Little.",
            canThrow = true,
            canConsume = true,
            isStoveIngredient = true,
            hasOverworldUses = true,
        };
        presets["RollingPin"] = new InventoryItem
        {
            itemName = "Rolling pin",
            damage = 50,
            durability = 15,
            toolTip = "",
            canAttack = true,
        };
        presets["SaltShaker"] = new InventoryItem
        {
            itemName = "Salt shaker",
            toolTip = "To rub salt in the wound.",
            damageScalesWithHP = true,
            isConsumedOnUse = true,
            damage = 30,
            canThrow = true,
            hasOverworldUses = true,
        };
        presets["SpicyPizza"] = new InventoryItem
        {
            itemName = "Spicy pizza",
            damage = 25,
            durability = 10,
            amountOfHits = 2,
            inflictOnFire = true,
            isFood = true,
            toolTip = "",
            canAttack = true,
            hasOverworldUses = true,
        };
        presets["Spife"] = new InventoryItem
        {
            itemName = "Spife",
            damage = 35,
            durability = 20,
            canAttack = true,
            healingBoost = 20,
            isMetal = true,
            isKnife = true,
            toolTip = "",
            isStoveIngredient = true,
        };
        presets["Spoon"] = new InventoryItem
        {
            itemName = "Spoon",
            damage = 20,
            durability = 10,
            canAttack = true,
            healingBoost = 10,
            isMetal = true,
            toolTip = "Great for if you're lonely in bed",
            isStoveIngredient = true,
        };
        presets["Sporf"] = new InventoryItem
        {
            itemName = "Sporf",
            damage = 60,
            durability = 30,
            canAttack = true,
            healingBoost = 30,
            isMetal = true,
            isKnife = true,
            toolTip = "Who needs multiple pieces of cutlery when you can use this!"
        };
        presets["Spork"] = new InventoryItem
        {
            itemName = "Spork",
            damage = 40,
            durability = 20,
            canAttack = true,
            healingBoost = 20,
            isMetal = true,
            toolTip = "",
            isStoveIngredient = true,
        };
        presets["StainlessSteelPot"] = new InventoryItem
        {
            itemName = "Stainless steel pot",
            isMetal = true,
            toolTip = "Cheaper drumset",
            isStoveIngredient = true,
        };
        presets["Sausage"] = new InventoryItem
        {
            itemName = "Sausage",
            damage = 50,
            durability = 15,
            isFood = true,
            toolTip = "",
            canAttack = true,
        };
        presets["Toaster"] = new InventoryItem
        {
            itemName = "Toaster",
            damage = 70,
            consumesBread = true,
            isFood = true,
            toolTip = "Very friendly bathing companion",
            canAttack = true,
            hasOverworldUses = true,
        };
        presets["Tomato"] = new InventoryItem
        {
            itemName = "Tomato",
            isConsumedOnUse = true,
            isStackable = true,
            maxStack = 10,
            damage = 30,
            isFood = true,
            toolTip = "An italian apple",
            canThrow = true,
            isStoveIngredient = true,
            hasOverworldUses = true,
        };
        presets["TomatoPizza"] = new InventoryItem
        {
            itemName = "Tomato pizza",
            damage = 25,
            durability = 10,
            amountOfHits = 2,
            inflictFakeBlood = true,
            isFood = true,
            toolTip = "",
            canAttack = true,
        };
        presets["Towel"] = new InventoryItem
        {
            itemName = "Towel",
            durabilityBoost = true,
            toolTip = "A towel is just about the most massively useful thing an interstellar hitchhiker can carry.",
            isStoveIngredient = true,
        };
        presets["WaterGlass"] = new InventoryItem
        {
            itemName = "Water bottle",
            damage = 10,
            curesOnFireWhenConsumed = true,
            isFood = true,
            toolTip = "Bo'ol o' wa'ah",
            canThrow = true,
            canConsume = true,
            isStoveIngredient = true,
            hasOverworldUses = true,
        };


        presets["BlessedCheese"] = new InventoryItem
        {
            itemName = "Blessed cheese",
            damage = 1500,
            durability = 42069,
            targetAll = true,
            seeEnemyInventories = true,
            isKnife = true,
            isMetal = true,
            inflictOnFire = true,
            inflictBlindness = true,
            inflictFakeBlood = true,
            inflictPoisoned = true,
            inflictStuck = true,
            isFood = true,
            toolTip = "You cheated to get this item =)",
            canAttack = true,
            hasOverworldUses = true,
        };
        presets["UndyingPan"] = new InventoryItem
        {
            itemName = "Undying pan",
            grantsImmortality = true,
            fireImmunity = true,
            toolTip = "You cheated to get this item =)"
        };

        Dictionary<string, InventoryItem> _newPresets = new Dictionary<string, InventoryItem>();
        foreach (KeyValuePair<string, InventoryItem> preset in presets)
        {
            string _name = preset.Key;
            InventoryItem _newItem = preset.Value;


            if (preset.Value.maxStack == 0) _newItem.maxStack = 1;
            if (preset.Value.amountLeft == 0) _newItem.amountLeft = 1;
            if (preset.Value.amountOfTargets == 0) _newItem.amountOfTargets = 1;
            if (preset.Value.amountOfHits == 0) _newItem.amountOfHits = 1;

            _newItem.itemModel = GetMeshByName(_name);
            _newItem.projectile = GetProjectileByName(_name);
            _newPresets[_name] = _newItem;
        }
        presets = _newPresets;
}
    GameObject GetMeshByName(string _name)
    {
        foreach(var model in itemModels) 
            if (model.name == _name) return model;

        return placeholderModel;
    }
    Rigidbody GetProjectileByName(string _name)
    {
        foreach (var projectile in projectiles)
            if (projectile.name.Replace("Projectile", "") == _name) return projectile;

        return placeholderProjectile;
    }
}
