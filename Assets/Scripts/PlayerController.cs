using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 playerDestination;
    public float moveSpeed = 1;

    Inventory inventory;
    private void Awake()
    {
        playerDestination = transform.position;
        inventory = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Inventory>();
    }
    void Update()
    {
        // Set destination on click
        if (Input.GetMouseButton(0))
        {
            //Translates mouse 2D position
            float _posX = Remap(0, 1, -1, 1, Input.mousePosition.x / Screen.width);
            float _posY = Remap(0, 1, -0.667f, 0.667f, Input.mousePosition.y / Screen.height);
            Vector3 _mousePosition = new(_posX, _posY, 0);

            // Casts in direction of mouse position
            if (Physics.Raycast(Camera.main.transform.position, _mousePosition,
                out RaycastHit rayHit, 1000, LayerMask.GetMask("Terrain"), QueryTriggerInteraction.Ignore))
            {
                Debug.DrawLine(Camera.main.transform.position, rayHit.point, Color.red, 1);
                playerDestination = rayHit.point;
            }
            Debug.DrawLine(Camera.main.transform.position, _mousePosition, Color.magenta, 1);
        }
        
        // Move to destination
        if ((playerDestination - transform.position).magnitude > 1)
        {
            Vector3 movement = (playerDestination - transform.position).normalized;
            this.transform.position += Time.deltaTime * moveSpeed * movement;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (inventory.currentInventory[Inventory.itemSelected].hasOverworldUses &&
                inventory.currentInventory[Inventory.itemSelected].projectile != null)
            {
                Rigidbody spawnedProjectile = Instantiate(inventory.currentInventory[Inventory.itemSelected].projectile, 
                    transform.position, Quaternion.identity);
                Debug.Log($"Spawned projectile, from {this}");
            }
        }
    }

    static float Remap(float oldRangeX, float oldRangeY, float newRangeX, float newRangeY, float value)
    {
        Mathf.InverseLerp(oldRangeX, oldRangeY, value);
        return Mathf.Lerp(newRangeX, newRangeY, value);
    }
}
