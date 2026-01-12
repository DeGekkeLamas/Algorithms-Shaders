using InventoryStuff;
using UnityEngine;

namespace Entities.Player
{
    /// <summary>
    /// Set values of projectile chart, enables it if held item is lobbed, disables it otherwise
    /// </summary>
    public class SetProjectileChart : MonoBehaviour
    {
        [SerializeField] MeshRenderer projectileChart;
        Material projectileChartMat;

        private void Awake()
        {
            projectileChartMat = projectileChart.material;
        }

        private void Update()
        {

            Vector3 relMousePos = PlayerController.GetMousePosition() - this.transform.position;
            SetChart(relMousePos);
        }

        void SetChart(Vector3 mousePos)
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
    }
}
