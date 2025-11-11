using InventoryStuff;
using UnityEngine;


[CreateAssetMenu(
    fileName = "FetchQuest",
    menuName = "ScriptableObjects/Quests/FetchQuest",
    order = 0)]
public class FetchQuest : Quest
{
    public InventoryItemData toCollect;

    private void OnValidate()
    {
        SetDescription();
    }

    public override void Initialize()
    {
        texture = toCollect.GetItem().ItemSprite;
        Inventory.instance.OnItemChanged += UpdateProgress;
        base.Initialize();
    }

    protected void UpdateProgress()
    {
        if (Inventory.instance.Contains(toCollect.GetItem() ) ) OnCompleted();
        InvokeOnProgressUpdated();

    }

    protected override void OnCompleted()
    {
        base.OnCompleted();
        Inventory.instance.OnItemChanged -= UpdateProgress;
    }

    protected override string SetDescription()
    {
        if (toCollect == null) return "No item set";

        string desc = $"Obtain {("aeiouAEIOU".Contains(toCollect.GetItem().itemName[0]) ? "an" : "a")} {toCollect.GetItem().itemName}";

        description = desc;
        return desc;
    }
}
