using NaughtyAttributes;
using UnityEngine;

namespace InventoryStuff
{
    public class InventoryItemTools : MonoBehaviour
    {
        [SerializeField] InventoryItemData item;
        InventoryItem Item => item.GetItem();

        [Button]
        void ExportTexture()
        {
            TextureExporter.ExportTexture(Item.ItemSprite, $"ItemSprite{item.name}");
        }

        [Button]
        void ExportSilhouetteTexture()
        {
            TextureExporter.ExportTexture(Item.ItemGrayscale, $"ItemSilhouetteSprite{item.name}");
        }
    }
}
