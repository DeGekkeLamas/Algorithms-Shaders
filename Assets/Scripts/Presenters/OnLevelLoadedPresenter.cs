using UnityEngine;

public abstract class OnLevelLoadedPresenter : MonoBehaviour
{
    protected GameManager gameManager;
    private void Awake()
    {
        gameManager = this.GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        gameManager.OnNewFloorLoaded += UpdateUI;
    }
    private void OnDisable()
    {
        gameManager.OnNewFloorLoaded += UpdateUI;
    }

    protected abstract void UpdateUI();
}
