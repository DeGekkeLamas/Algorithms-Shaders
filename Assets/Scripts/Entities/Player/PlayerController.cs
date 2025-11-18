using InventoryStuff;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Player
{
    /// <summary>
    /// Base for playercontroller, override to add a way to move player
    /// </summary>
    public abstract class PlayerController : Entity
    {
        public float projectileForce = 5;

        [Header("References")]
        public Rigidbody pickupSpawned;
        public MeshRenderer projectileChart;
        public WeaponHandle meleeWeaponHandle;
        Material projectileChartMat;
        public static PlayerController instance;
        protected override void Awake()
        {
            if (instance == null) instance = this;
            else
            {
                instance.transform.position = this.transform.position;
                Destroy(this.gameObject);
            }

            base.Awake();
            projectileChartMat = projectileChart.material;
        }
        protected override void Start()
        {
            base.Start();
            transform.parent = GameManager.instance.transform;
        }
        protected virtual void Update()
        {
            UpdateAction(Input.GetMouseButtonDown(0), Input.GetMouseButton(0), Input.GetKeyDown(KeyCode.E));
        }
        protected void UpdateAction(bool interactInput, bool interactInputStay, bool dropItemKey)
        {
            bool canUseItem = true;
            Vector3 relMousePos = GetMousePosition() - this.transform.position;

            // Checks for mouse position on right mouse click for interaction
            if (interactInput)
            {
                (bool, RaycastHit) rayHit = GetCamCast(~LayerMask.GetMask("Terrain"));
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
                itemSelected.UseItem(this, relMousePos);
            }

            // Drop currently held item
            if (dropItemKey && itemSelected != null)
            {
                DropHeldItem();
            }

            // Set projectile chart data
            SetProjectileChart(relMousePos);
        }

        void DropHeldItem()
        {
            InventoryItem itemSelected = Inventory.instance.currentInventory[Inventory.itemSelected].item;
            Debug.Log($"Dropped {itemSelected.itemName}, from {this}");
            Rigidbody droppedItem = Instantiate(pickupSpawned, this.transform.position + transform.forward, Quaternion.identity);
            droppedItem.gameObject.GetComponent<PickupItem>().itemToGive = Inventory.instance.currentInventory[Inventory.itemSelected].item;
            Inventory.instance.RemoveFromStack(Inventory.itemSelected);
        }

        void SetProjectileChart(Vector3 mousePos)
        {
            InventoryItem item = Inventory.instance.currentInventory[Inventory.itemSelected].item;
            RangedWeapon itemR = item as RangedWeapon;
            if (item != null && itemR != null && itemR.projectile.useGravity)
            {
                projectileChart.gameObject.SetActive(true);
            }
            else
            {
                projectileChart.gameObject.SetActive(false);
                return;
            }

            Vector3 nMousePos = mousePos;
            nMousePos.y = 0;
            nMousePos = nMousePos.normalized;
            Vector3 scale = projectileChart.transform.localScale;

            projectileChartMat.SetVector("_Target", mousePos);
            // Object position
            projectileChart.transform.position = this.transform.position + new Vector3(
                nMousePos.x * 0.5f * scale.x,
                scale.y * 0.5f,
                nMousePos.z * 0.5f * scale.z);
            // Object rotation
            var rotation = Quaternion.LookRotation(nMousePos, Vector3.up).eulerAngles + new Vector3(0, -90, 0);
            projectileChart.transform.eulerAngles = new(0, rotation.y, 0);
        }

        Vector3 GetMousePosition()
        {
            (bool, RaycastHit) rayHit = GetCamCast(~LayerMask.GetMask("Player"));
            return rayHit.Item2.point;
        }
        protected (bool, RaycastHit) GetCamCast(int layermask)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hitSomething = Physics.Raycast(mouseRay,
                out RaycastHit rayHit, 1000, layermask, QueryTriggerInteraction.Ignore);
            return (hitSomething, rayHit);
        }

        protected override void Death()
        {
            base.Death();
            Debug.Log("You suck at this game LMAO");
        }
    }
}
