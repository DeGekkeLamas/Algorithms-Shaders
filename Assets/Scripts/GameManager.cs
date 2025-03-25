using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool isInBattle;
    Inventory inventory;

    [Header("UI")]
    public GameObject hotBar;
    public GameObject itemDescription;
    public Vector3 battleUIPosition;
    public Vector3 overworldUIPosition;

    bool shouldUpdateInventory;

    private void Awake() => inventory = GetComponent<Inventory>();
    private void Update()
    {
        if (shouldUpdateInventory)
        {
            for (int i = 0; i < inventory.currentInventory.Length; i++) inventory.UpdateInventory(i);
            shouldUpdateInventory = false;
        }
    }

    [ContextMenu("Start battle")]
    public void StartBattle(Projectile hitProjectile = null)
    {
        isInBattle = true;
        itemDescription.SetActive(true);
        hotBar.transform.position = battleUIPosition;

        shouldUpdateInventory = true;

        // Destroy all existing projectiles
        for(int i = Projectile.existingProjectiles.Count; Projectile.existingProjectiles.Count > 1; i--)
        {
            if (Projectile.existingProjectiles[Mathf.Clamp(i, 0, Projectile.existingProjectiles.Count - 1)] != hitProjectile)
            {
                Destroy(Projectile.existingProjectiles[Mathf.Clamp(i, 0, Projectile.existingProjectiles.Count - 1)].gameObject);
                Projectile.existingProjectiles.RemoveAt(Mathf.Clamp(i, 0, Projectile.existingProjectiles.Count - 1));
            }
        }
        Debug.Log("Started battle");
    }

    [ContextMenu("End battle")]
    public void EndBattle()
    {
        isInBattle = false;
        itemDescription.SetActive(false);
        hotBar.transform.position = overworldUIPosition;
        for (int i = 0; i < inventory.currentInventory.Length; i++) inventory.UpdateInventory(i);
        Debug.Log("Ended battle");
    }
}
