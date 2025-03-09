using System.Collections.Generic;
using UnityEngine;

public class ItemPresets : MonoBehaviour
{
    public static Dictionary<string, InventoryItem> presets = new Dictionary<string, InventoryItem>();
    public GameObject[] itemModels;
    public GameObject placeholderModel;
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
            toolTip = "Will you use this as gun or as a phone? Will you pick the dark side or the light side?"
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
            canThrow = true
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
        };
        presets["GrilledChicken"] = new InventoryItem
        {
            itemName = "Grilled chicken",
            hpHealed = 60,
            toolTip = "",
            canConsume = true,
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
            toolTip = "One pan to rule them all..."
        };
        presets["PepperShaker"] = new InventoryItem
        {
            itemName = "Pepper shaker",
            isConsumedOnUse = true,
            inflictOnFire = true,
            targetAll = true,
            toolTip = "Tasty flavor ashes!",
            canThrow = true
        };
        presets["PipingBag"] = new InventoryItem
        {
            itemName = "Piping bag",
            damage = 25,
            amountOfTargets = 3,
            toolTip = "",
            canAttack = true
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
            canThrow = true
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
            toolTip = "Great for if youre lonely in bed",
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
            canAttack = true
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
            canAttack = true
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


/**
    public InventoryItem apron = new InventoryItem
    {
        itemName = "Apron",
        foodResistanceBoost = true,
        toolTip = "What does your grandma's death have to do with the blo- tomato stains on this?"
    };
    public InventoryItem baguette = new InventoryItem
    {
        itemName = "Baguette",
        isFood = true,
        damage = 45,
        durability = 10,
        toolTip = "A French katana, or a French Japanese sword.",
        canAttack = true,
    };
    public InventoryItem banana = new InventoryItem
    {
        itemName = "Banana",
        isConsumedOnUse = true,
        damage = 80,
        hpHealed = 40,
        canAttack = true,
        canConsume = true,
        isFood = true,
        toolTip = "Will you use this as gun or as a phone? Will you pick the dark side or the light side?"
    };
    public InventoryItem bread = new InventoryItem
    {
        itemName = "Bread",
        isStackable = true,
        maxStack = 5,
        toolTip = "Hansel and Gretel GPS.",
        isStoveIngredient = true,
    };
    public InventoryItem butter = new InventoryItem
    {
        itemName = "Butter",
        damage = 10,
        isConsumedOnUse = true,
        inflictStuck = true,
        toolTip = "It's like sunscreen but better!",
        canThrow = true,
    };
    public InventoryItem cake = new InventoryItem
    {
        itemName = "Cake",
        isConsumedOnUse = true,
        damage = 25,
        inflictBlindness = true,
        isFood = true,
        toolTip = "Just as caked out as your mom.",
        canThrow = true,
    };
    public InventoryItem cakeKnife = new InventoryItem
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
    public InventoryItem cheese = new InventoryItem
    {
        itemName = "Cheese",
        isConsumedOnUse = true,
        hpHealed = 40,
        toolTip = "Say cheese when taking a picture",
        canConsume = true,
        isStoveIngredient = true,
    };
    public InventoryItem cheesePizza = new InventoryItem
    {
        itemName = "Cheese pizza",
        damage = 25,
        durability = 10,
        amountOfHits = 2,
        onAttackHealHP = true,
        isFood = true,
        toolTip = "You can never have enough cheese.",
        canAttack = true,
    };
    public InventoryItem chefHat = new InventoryItem
    {
        itemName = "Chef hat",
        foodBoost = true,
        toolTip = ""
    };
    public InventoryItem cryingPan = new InventoryItem
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
    public InventoryItem cuttingBoard = new InventoryItem
    {
        itemName = "Cutting board",
        durability = 50,
        knifeBoost = true,
        toolTip = "",
        canBlock = true
    };
    public InventoryItem dryingPan = new InventoryItem
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
    public InventoryItem dyingPan = new InventoryItem
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
    public InventoryItem egg = new InventoryItem
    {
        itemName = "Egg",
        isConsumedOnUse = true,
        isStackable = true,
        maxStack = 10,
        damage = 40,
        isFood = true,
        toolTip = "One of the most popular forms of child to eat",
        canThrow = true
    };
    public InventoryItem emptyMicrowave = new InventoryItem
    {
        itemName = "Empty microwave",
        isEmptyMicrowave = true,
        toolTip = "",
    };
    public InventoryItem fork = new InventoryItem
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
    public InventoryItem fryingPan = new InventoryItem
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
    public InventoryItem grilledCheeseSandwich = new InventoryItem
    {
        itemName = "Grilled cheese sandwich",
        isConsumedOnUse = true,
        hpHealed = 70,
        toolTip = "Nice and warm like a witch in the sand",
        canConsume = true,
    };
    public InventoryItem grilledChicken = new InventoryItem
    {
        itemName = "Grilled chicken",
        hpHealed = 60,
        toolTip = "",
        canConsume = true,
    };
    public InventoryItem identifyingPan = new InventoryItem
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
    public InventoryItem jalapeno = new InventoryItem
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
    };
    public InventoryItem ketchupBottle = new InventoryItem
    {
        itemName = "Ketchup bottle",
        isConsumedOnUse = true,
        inflictFakeBlood = true,
        hpHealed = 30,
        damage = 30,
        toolTip = "Reminds you of the dentist...",
        canThrow = true,
        canConsume = true,
    };
    public InventoryItem knife = new InventoryItem
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
    public InventoryItem knork = new InventoryItem
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
    public InventoryItem loadedMicrowave = new InventoryItem
    {
        itemName = "Loaded microwave",
        isConsumedOnUse = true,
        targetAll = true,
        damage = 80,
        isFood = true,
        toolTip = "This thing is the bomb!",
        canThrow = true,
    };
    public InventoryItem mystifyingPan = new InventoryItem
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
        toolTip = "One pan to rule them all..."
    };
    public InventoryItem pepperShaker = new InventoryItem
    {
        itemName = "Pepper shaker",
        isConsumedOnUse = true,
        inflictOnFire = true,
        targetAll = true,
        toolTip = "Tasty flavor ashes!",
        canThrow = true
    };
    public InventoryItem pipingBag = new InventoryItem
    {
        itemName = "Piping bag",
        damage = 25,
        amountOfTargets = 3,
        toolTip = "",
        canAttack = true
    };
    public InventoryItem pizza = new InventoryItem
    {
        itemName = "Pizza",
        damage = 25,
        durability = 10,
        amountOfHits = 2,
        isFood = true,
        toolTip = "An italian frisbee",
        canAttack = true,
        isStoveIngredient = true,
    };
    public InventoryItem plate = new InventoryItem
    {
        itemName = "Plate",
        isConsumedOnUse = true,
        damage = 35,
        damageAfterBlock = 15,
        toolTip = "Good thing it doesn't fly back when thrown as frisbee!",
        canThrow = true,
        canBlock = true,
    };
    public InventoryItem rawChicken = new InventoryItem
    {
        itemName = "Raw chicken",
        damage = 20,
        hpHealed = 10,
        inflictPoisoned = true,
        toolTip = "Alternate ending to Chicken Little.",
        canThrow = true,
        canConsume = true,
        isStoveIngredient = true,
    };
    public InventoryItem rollingPin = new InventoryItem
    {
        itemName = "Rolling pin",
        damage = 50,
        durability = 15, 
        toolTip = "",
        canAttack = true,
    };
    public InventoryItem saltShaker = new InventoryItem
    {
        itemName = "Salt shaker",
        toolTip = "To rub salt in the wound.",
        damageScalesWithHP = true,
        isConsumedOnUse = true,
        damage = 30,
        canThrow = true
    };
    public InventoryItem spicyPizza = new InventoryItem
    {
        itemName = "Spicy pizza",
        damage = 25,
        durability = 10,
        amountOfHits = 2,
        inflictOnFire = true,
        isFood = true,
        toolTip = "",
        canAttack = true,
    };
    public InventoryItem spife = new InventoryItem
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
    public InventoryItem spoon = new InventoryItem
    {
        itemName = "Spoon",
        damage = 20,
        durability = 10,
        canAttack = true,
        healingBoost = 10,
        isMetal = true,
        toolTip = "Great for if youre lonely in bed",
        isStoveIngredient = true,
    };
    public InventoryItem sporf = new InventoryItem
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
    public InventoryItem spork = new InventoryItem
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
    public InventoryItem stainlessSteelPot = new InventoryItem
    {
        itemName = "Stainless steel pot",
        isMetal = true,
        toolTip = "Cheaper drumset",
        isStoveIngredient = true,
    };
    public InventoryItem sausage = new InventoryItem
    {
        itemName = "Sausage",
        damage = 50,
        durability = 15,
        isFood = true,
        toolTip = "",
        canAttack = true,
    };
    public InventoryItem toaster = new InventoryItem
    {
        itemName = "Toaster",
        damage = 70,
        consumesBread = true,
        isFood = true,
        toolTip = "Very friendly bathing companion",
        canAttack = true
    };
    public InventoryItem tomato = new InventoryItem
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
    };
    public InventoryItem tomatoPizza = new InventoryItem
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
    public InventoryItem towel = new InventoryItem
    {
        itemName = "Towel",
        durabilityBoost = true,
        toolTip = "A towel is just about the most massively useful thing an interstellar hitchhiker can carry.",
        isStoveIngredient = true,
    };
    public InventoryItem waterBottle = new InventoryItem
    {
        itemName = "Water bottle",
        damage = 10,
        curesOnFireWhenConsumed = true,
        isFood = true,
        toolTip = "Bo'ol o' wa'ah",
        canThrow = true,
        canConsume = true,
        isStoveIngredient = true,
    };



    [Header("Cheat items")]
    public InventoryItem blessedCheese = new InventoryItem
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
        canAttack = true
    };
    public InventoryItem undyingPan = new InventoryItem
    {
        itemName = "Undying pan",
        grantsImmortality = true,
        fireImmunity = true,
        toolTip = "You cheated to get this item =)"
    };**/
}
