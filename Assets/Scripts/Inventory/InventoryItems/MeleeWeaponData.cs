using System.Collections;
using UnityEngine;
using Entities;
using Entities.Player;
using Entities.StatusEffects;

namespace InventoryStuff
{
    [CreateAssetMenu(
        fileName = "MeleeItem",
        menuName = "ScriptableObjects/Items/MeleeItem",
        order = 0)]
    public class MeleeWeaponData : InventoryItemData
    {
        public MeleeWeapon item = new();
        public override InventoryItem GetItem() { return item; }
    }

    [System.Serializable]
    public class MeleeWeapon : InventoryItem
    {
        [Header("Type specific")]
        public float damage;
        public StatusEffect[] effectsApplied;

        public float swingTime = .5f;
        public float swingAngle = 45;
        [Tooltip("Size of the collider on the object")]
        public float distane = 1;
        [Tooltip("Distance of the physical model from the player")]
        public float objectDistance = 1;
        public float objectScale = 1;
        public Vector3 modelRotation;
        public override void UseItem(Entity source, Vector3 inputDir)
        {
            Debug.Log($"Swung {itemName}");
            PlayerController player = source as PlayerController;
            source.StartCoroutine(SwingWeapon(player, inputDir));
        }

        IEnumerator SwingWeapon(PlayerController source, Vector3 inputDir)
        {
            // Initialize
            source.meleeWeaponHandle.gameObject.SetActive(true);
            float originalSpeed = source.moveSpeed;
            source.ChangeMoveSpeed(0);
            canUseItem = false;
            GameObject model = MonoBehaviour.Instantiate(itemModel, source.meleeWeaponHandle.transform.position,
                source.meleeWeaponHandle.transform.rotation, source.meleeWeaponHandle.transform);
            model.transform.localScale *= objectScale;
            source.meleeWeaponHandle.handleCollider.size = new(.2f, 1, distane);
            source.meleeWeaponHandle.damager.damage = damage;
            inputDir.y = 0;

            // Swing
            for (float i = 0; i < swingTime; i += Time.deltaTime)
            {
                // Position
                Vector3 offset = inputDir.normalized * objectDistance;
                float currentAngle = MathTools.Remap(0, swingTime, -swingAngle, swingAngle, i);
                offset = VectorMath.RotateVectorXZ(offset, currentAngle);
                // Rotation
                float rotation = Quaternion.LookRotation(offset, Vector3.up).eulerAngles.y;

                source.meleeWeaponHandle.transform.position = source.transform.position + offset + Vector3.up;
                source.meleeWeaponHandle.transform.eulerAngles = new Vector3(0,rotation,0) + modelRotation;
                yield return null;
            }

            // Exit
            source.ChangeMoveSpeed(originalSpeed);
            MonoBehaviour.Destroy(model);
            source.meleeWeaponHandle.damager.damage = 0;
            source.meleeWeaponHandle.gameObject.SetActive(false);
            canUseItem = true;
        }
        
    }
}