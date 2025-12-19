using InventoryStuff;
using UnityEngine;

namespace Entities.Player
{
    public class OnClickInteractWithObjectOrUseItem : MonoBehaviour
    {
        [SerializeField] MouseType input = MouseType.Left;
        Entity boundEntity;
        private void Awake()
        {
            boundEntity = this.GetComponent<Entity>();
        }

        void Update()
        {
            bool canUseItem = true;
            Vector3 relMousePos = PlayerController.GetMousePosition() - this.transform.position;
            bool interactInput = input == MouseType.Left ? Input.GetMouseButtonDown(0) : Input.GetMouseButtonDown(1);

            // Checks for mouse position on right mouse click for interaction
            if (interactInput)
            {
                (bool, RaycastHit) rayHit = PlayerController.GetCamCast(~LayerMask.GetMask("Terrain"));
                if (rayHit.Item1)
                {
                    Debug.DrawLine(Camera.main.transform.position, rayHit.Item2.point, Color.blue, 1);

                    if (rayHit.Item2.collider.gameObject.TryGetComponent(out IInteractible interactible))
                    {
                        interactible.OnInteract();
                        canUseItem = false;
                    }
                }
            }

            // Use item if it has interactions
            InventoryItem itemSelected = Inventory.instance.currentInventory[Inventory.itemSelected].item;
            if (itemSelected != null && (interactInput) /*|| (interactInputStay && itemSelected.autoFire)*/ &&
                itemSelected.canUseItem && canUseItem)
            {
                itemSelected.UseItem(boundEntity, relMousePos);
            }
        }
    }

    enum MouseType { Left, Right }
}
