using Entities;
using Entities.Player;
using Entities.StatusEffects;
using System;
using System.Collections;
using UnityEngine;

namespace InventoryStuff
{
    [CreateAssetMenu(
        fileName = "AOEAttackItem",
        menuName = "ScriptableObjects/Items/Special/AOEAttackItem",
        order = 0)]
    public class AOEAttackItemData : InventoryItemData
    {
        public AOEAttackItem item = new();
        public override InventoryItem GetItem() { return item; }
    }

    [Serializable]
    public class AOEAttackItem : InventoryItem
    {
        public float damage;
        public StatusEffect[] effectsApplied;
        public float distance = 3;
        public OnTriggerDamageEntity toSpawn;
        public float duration = .5f;

        public override void UseItem(Entity source, Vector3 inputDir)
        {
            Debug.Log($"Swung {itemName}");
            PlayerController player = source as PlayerController;
            source.StartCoroutine(SwingWeapon(player, inputDir));
        }

        IEnumerator SwingWeapon(PlayerController source, Vector3 inputDir)
        {
            // Initialize
            float originalSpeed = source.moveSpeed;
            source.ChangeMoveSpeed(0);
            canUseItem = false;

            yield return UseWeaponAnimation(source, inputDir);

            // Exit
            source.ChangeMoveSpeed(originalSpeed);
            canUseItem = true;
        }

        IEnumerator UseWeaponAnimation(PlayerController source, Vector3 inputDir)
        {
            OnTriggerDamageEntity spawned = MonoBehaviour.Instantiate(toSpawn, source.transform.position + new Vector3(0, .5f, 0), source.transform.rotation);
            toSpawn.transform.localScale = new(distance, 1, distance);
            spawned.damage = damage * source.strength;
            spawned.AddException(source);

            yield return new WaitForSeconds(duration);
        }

        public override string GetItemDescription()
        {
            string description = base.GetItemDescription();

            // Damage
            description += $"Deals {damage} AOE damage.\n";
            // Effects applied
            foreach (StatusEffect effect in effectsApplied)
            {
                description += $"Inflicts {effect.name}.\n";
            }

            return description;
        }
    }
}
