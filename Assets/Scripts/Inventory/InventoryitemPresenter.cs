using InventoryStuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Presenter for inventoryitems
/// </summary>
public class InventoryitemPresenter : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] RawImage sprite;
    [SerializeField] Texture2D emptySlotTex;
    [SerializeField] TMP_Text quantityCounter;
    [SerializeField] Slider durabilitySlider;

    [Header("Border")]
    [SerializeField] RawImage border;
    [SerializeField] Texture2D borderUnselected;
    [SerializeField] Texture2D borderSelected;

    private void Start()
    {
        Inventory.instance.OnItemChanged += UpdateDisplay;
        Inventory.OnSeletecItemSwitched += SetBorder;

        UpdateDisplay();
        SetBorder();
    }

    void UpdateDisplay()
    {
        ItemUniqueStats invItem = Inventory.instance.currentInventory[index];
        // Sprite
        if (sprite != null)
        {
            if (invItem.item != null)
            {
                sprite.texture = invItem.item.ItemSprite;
            }
            else sprite.texture = emptySlotTex;
        }
        // Counter
        if (quantityCounter != null)
        {
            if (invItem.item != null && invItem.item.maxStack > 1)
            {
                quantityCounter.gameObject.SetActive(true);
                quantityCounter.text = $"{invItem.quantityLeft}";
            }
            else
            {
                quantityCounter.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Sets border for active item
    /// </summary>
    void SetBorder()
    {
        if (border != null)
        {
            if (Inventory.itemSelected == index) border.texture = borderSelected;
            else border.texture = borderUnselected;
        }
    }
}
