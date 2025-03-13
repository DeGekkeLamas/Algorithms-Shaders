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

    private void Awake() => inventory = GetComponent<Inventory>();

    [ContextMenu("Start battle")]
    void StartBattle()
    {
        isInBattle = true;
        itemDescription.SetActive(true);
        hotBar.transform.position = battleUIPosition;
        inventory.UpdateInventory();
    }

    [ContextMenu("End battle")]
    void EndBattle()
    {
        isInBattle = false;
        itemDescription.SetActive(false);
        hotBar.transform.position = overworldUIPosition;
        inventory.UpdateInventory();
    }
}
