using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    Vector3 playerDestination;
    public float moveSpeed = 1;
    public float projectileForce = 5;

    public Rigidbody pickupSpawned;
    public static PlayerController playerReference; 
    NavMeshAgent navMeshAgent;

    Inventory inventory;
    private void Awake()
    {
        playerDestination = transform.position;
        inventory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Inventory>();
        playerReference = this;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        bool canUseItem = true;

        // Set destination on click
        if (Input.GetMouseButton(0))
        {
            //Translates mouse 2D position
            float _posX = Remap(0, 1, -1, 1, Input.mousePosition.x / Screen.width);
            float _posY = Remap(0, 1, -0.667f, 0.667f, Input.mousePosition.y / Screen.height);
            Vector3 _mousePosition = new(_posX, _posY, 0);

            // Casts in direction of mouse position
            if (Physics.Raycast(Camera.main.transform.position, VectorMath.RotateVector3(_mousePosition + transform.forward, Camera.main.transform.eulerAngles) 
                , out RaycastHit rayHit, 1000, LayerMask.GetMask("Terrain"), QueryTriggerInteraction.Ignore))
            {
                //Debug.DrawLine(Camera.main.transform.position, rayHit.point, Color.red, 1);
                navMeshAgent.SetDestination(rayHit.point);
                ///playerDestination = rayHit.point;
            }
        }

        // Checks for mouse position on right mouse click for interaction
        if (Input.GetMouseButtonDown(1))
        {
            //Translates mouse 2D position
            float _posX = Remap(0, 1, -1, 1, Input.mousePosition.x / Screen.width);
            float _posY = Remap(0, 1, -0.667f, 0.667f, Input.mousePosition.y / Screen.height);
            Vector3 _mousePosition = new(_posX, _posY, 0);

            // Casts in direction of mouse position
            if (Physics.Raycast(Camera.main.transform.position, VectorMath.RotateVector3(_mousePosition + transform.forward, Camera.main.transform.eulerAngles),
                out RaycastHit otherRayHit, 1000, ~LayerMask.GetMask("Terrain"), QueryTriggerInteraction.Collide))
            {
                Debug.DrawLine(Camera.main.transform.position, otherRayHit.point, Color.blue, 1);

                if (otherRayHit.collider.gameObject.TryGetComponent<PickupItem>(out PickupItem item))
                {
                    bool couldAdd = inventory.AddItem(item.itemToGive);
                    if (couldAdd) Destroy(otherRayHit.collider.gameObject);
                    canUseItem = false;
                }
            }
        }

        // Use item if it has interactions
        if (Input.GetMouseButtonDown(1) && canUseItem || Input.GetMouseButton(1) && inventory.currentInventory[Inventory.itemSelected].autoFire &&
            inventory.currentInventory[Inventory.itemSelected].cooldownLeft <= 0 && canUseItem)
        {
            if (inventory.currentInventory[Inventory.itemSelected].hasOverworldUses &&
                inventory.currentInventory[Inventory.itemSelected].projectile != null &&
                inventory.currentInventory[Inventory.itemSelected].cooldownLeft <= 0)
            {
                // shot projectiles
                Rigidbody spawnedProjectile = Instantiate(inventory.currentInventory[Inventory.itemSelected].projectile,
                    transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                if (!spawnedProjectile.GetComponent<Projectile>().useGravity)
                { // Straight projectile
                    spawnedProjectile.linearVelocity = (GetMousePosition() - this.transform.position).normalized *
                        spawnedProjectile.GetComponent<Projectile>().projectileSpeed;
                    spawnedProjectile.linearVelocity = new(spawnedProjectile.linearVelocity.x, 0, spawnedProjectile.linearVelocity.z);
                }
                else // Lobbed projectile
                {
                    spawnedProjectile.linearVelocity = (GetMousePosition() - this.transform.position) *
                        spawnedProjectile.GetComponent<Projectile>().projectileSpeed;
                }

                inventory.currentInventory[Inventory.itemSelected].cooldownLeft =
                    inventory.currentInventory[Inventory.itemSelected].cooldown;
                if (inventory.currentInventory[Inventory.itemSelected].isConsumedOnUse) 
                    inventory.RemoveFromStack(Inventory.itemSelected);

                Debug.Log($"Spawned projectile, from {this}");
            }
        }
        // Drop currently held item
        if (Input.GetKeyDown(KeyCode.E) && !inventory.currentInventory[Inventory.itemSelected].slotIsEmty)
        {
            Debug.Log($"Dropped {inventory.currentInventory[Inventory.itemSelected].itemName}, from {this}");
            Rigidbody droppedItem = Instantiate(pickupSpawned, this.transform.position + transform.forward, Quaternion.identity);
            droppedItem.gameObject.GetComponent<PickupItem>().itemToGive = inventory.currentInventory[Inventory.itemSelected];
            inventory.RemoveFromStack(Inventory.itemSelected);
        }
        /**
        // Move to destination
        if ((playerDestination - transform.position).magnitude > 1)
        {
            Vector3 movement = (playerDestination - transform.position).normalized;
            this.transform.position += Time.deltaTime * moveSpeed * movement;
        }**/
    }

    Vector3 GetMousePosition()
    {
        //Translates mouse 2D position
        float _posX = Remap(0, 1, -1, 1, Input.mousePosition.x / Screen.width);
        float _posY = Remap(0, 1, -0.667f, 0.667f, Input.mousePosition.y / Screen.height);
        Vector3 _mousePosition = new(_posX, _posY, 0);

        if (Physics.Raycast(Camera.main.transform.position, VectorMath.RotateVector3(_mousePosition + transform.forward, Camera.main.transform.eulerAngles),
            out RaycastHit rayHit, 1000, ~LayerMask.GetMask("Player"), QueryTriggerInteraction.Ignore))
        {

        }
        return rayHit.point;
    }

    static float Remap(float oldRangeX, float oldRangeY, float newRangeX, float newRangeY, float value)
    {
        Mathf.InverseLerp(oldRangeX, oldRangeY, value);
        return Mathf.Lerp(newRangeX, newRangeY, value);
    }
}
