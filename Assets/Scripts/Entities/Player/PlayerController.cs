using InventoryStuff;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;

/// <summary>
/// Base for playercontroller, override to add a way to move player
/// </summary>
public class PlayerController : Entity
{
    Vector3 playerDestination;
    public float projectileForce = 5;

    [Header("References")]
    public Rigidbody pickupSpawned;
    public MeshRenderer projectileChart;
    public GameObject meleeWeaponHandle;
    Material projectileChartMat;
    public static PlayerController instance;
    protected override void Awake()
    {
        base.Awake();
        playerDestination = transform.position;
        instance = this;
        projectileChartMat = projectileChart.material;
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
        projectileChart.transform.eulerAngles = new(0,rotation.y,0);
    }

    Vector3 GetMousePosition()
    {
        (bool, RaycastHit) rayHit = GetCamCast(~LayerMask.GetMask("Player"));
        return rayHit.Item2.point;
    }
    protected (bool, RaycastHit) GetCamCast(int layermask)
    {
        //Translates mouse 2D position
        float _posX = Remap(0, 1, -1, 1, Input.mousePosition.x / Screen.width);
        float _posY = Remap(0, 1, -0.667f, 0.667f, Input.mousePosition.y / Screen.height);
        Vector3 _mousePosition = new(_posX, _posY, 0);

        bool hitSomething = Physics.Raycast(Camera.main.transform.position, 
            VectorMath.RotateVector3(_mousePosition + transform.forward, Camera.main.transform.eulerAngles),
            out RaycastHit rayHit, 1000, layermask, QueryTriggerInteraction.Ignore);
        return (hitSomething, rayHit);
    }

    static float Remap(float oldRangeX, float oldRangeY, float newRangeX, float newRangeY, float value)
    {
        Mathf.InverseLerp(oldRangeX, oldRangeY, value);
        return Mathf.Lerp(newRangeX, newRangeY, value);
    }

    protected override void Death()
    {
        Debug.Log("You suck at this game LMAO");
    }
}
