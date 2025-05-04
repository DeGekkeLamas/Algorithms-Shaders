using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    Vector3 playerDestination;
    public float moveSpeed = 1;
    public float projectileForce = 5;

    public Rigidbody pickupSpawned;
    public MeshRenderer projectileChart;
    Material projectileChartMat;
    public static PlayerController playerReference; 
    NavMeshAgent navMeshAgent;

    Inventory inventory;
    private void Awake()
    {
        playerDestination = transform.position;
        inventory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Inventory>();
        playerReference = this;
        navMeshAgent = GetComponent<NavMeshAgent>();
        projectileChartMat = projectileChart.material;
    }
    void Update()
    {
        bool canUseItem = true;
        Vector3 relMousePos = GetMousePosition() - this.transform.position;

        // Set destination on click
        if (Input.GetMouseButton(0))
        {
            (bool, RaycastHit) rayHit = GetCamCast(LayerMask.GetMask("Terrain"));
            if (rayHit.Item1)
            {
                //Debug.DrawLine(Camera.main.transform.position, rayHit.point, Color.red, 1);
                navMeshAgent.SetDestination(rayHit.Item2.point);
                ///playerDestination = rayHit.point;
            }
        }

        // Checks for mouse position on right mouse click for interaction
        if (Input.GetMouseButtonDown(1))
        {
            (bool, RaycastHit) otherRayHit = GetCamCast(~LayerMask.GetMask("Terrain"));
            if (otherRayHit.Item1)
            {
                Debug.DrawLine(Camera.main.transform.position, otherRayHit.Item2.point, Color.blue, 1);

                if (otherRayHit.Item2.collider.gameObject.TryGetComponent<PickupItem>(out PickupItem item))
                {
                    bool couldAdd = inventory.AddItem(item.itemToGive);
                    if (couldAdd) Destroy(otherRayHit.Item2.collider.gameObject);
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
                    transform.position + new Vector3(0, 1, 0), Quaternion.LookRotation(relMousePos));
                if (!spawnedProjectile.GetComponent<Projectile>().useGravity)
                { // Straight projectile
                    spawnedProjectile.linearVelocity = relMousePos.normalized *
                        spawnedProjectile.GetComponent<Projectile>().projectileSpeed;
                    spawnedProjectile.linearVelocity = new(spawnedProjectile.linearVelocity.x, 0, spawnedProjectile.linearVelocity.z);
                }
                else // Lobbed projectile
                {
                    spawnedProjectile.linearVelocity = relMousePos *
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
        // Set projectile chart data
        projectileChartMat.SetVector("_Target", relMousePos);
        relMousePos.y = 0;
        projectileChart.transform.position = this.transform.position + new Vector3(
            (relMousePos.normalized * 0.5f * projectileChart.transform.localScale.x).x,
            projectileChart.transform.localScale.y * 0.5f,
            (relMousePos.normalized * 0.5f * projectileChart.transform.localScale.z).z);

        var rotation = Quaternion.LookRotation(relMousePos.normalized, Vector3.up).eulerAngles + new Vector3(0,-90,0);
        projectileChart.transform.eulerAngles = rotation;
    }

    Vector3 GetMousePosition()
    {
        (bool, RaycastHit) rayHit = GetCamCast(~LayerMask.GetMask("Player"));
        return rayHit.Item2.point;
    }
    (bool, RaycastHit) GetCamCast(int layermask)
    {
        //Translates mouse 2D position
        float _posX = Remap(0, 1, -1, 1, Input.mousePosition.x / Screen.width);
        float _posY = Remap(0, 1, -0.667f, 0.667f, Input.mousePosition.y / Screen.height);
        Vector3 _mousePosition = new(_posX, _posY, 0);

        bool hitSomething = Physics.Raycast(Camera.main.transform.position, VectorMath.RotateVector3(_mousePosition + transform.forward, Camera.main.transform.eulerAngles),
            out RaycastHit rayHit, 1000, layermask, QueryTriggerInteraction.Ignore);
        return (hitSomething, rayHit);
    }

    static float Remap(float oldRangeX, float oldRangeY, float newRangeX, float newRangeY, float value)
    {
        Mathf.InverseLerp(oldRangeX, oldRangeY, value);
        return Mathf.Lerp(newRangeX, newRangeY, value);
    }
}
