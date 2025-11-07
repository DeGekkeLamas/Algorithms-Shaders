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
