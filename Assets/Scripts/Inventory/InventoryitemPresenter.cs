using InventoryStuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryitemPresenter : MonoBehaviour
{
    public int index;
    public RawImage sprite;
    public Texture2D emptySlotTex;
    public TMP_Text quantityCounter;
    public Slider durabilitySlider;

    [Header("Border")]
    public RawImage border;
    public Texture2D borderUnselected;
    public Texture2D borderSelected;

    private void Start()
    {
        Inventory.instance.onItemChanged += UpdateDisplay;
        Inventory.instance.onSeletecItemSwitched += SetBorder;

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
                if (invItem.item.itemSprite != null)
                    sprite.texture = invItem.item.itemSprite;
                else
                    sprite.texture = invItem.item.SetSprite();
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
