using Entities;
using Entities.Player;
using Entities.StatusEffects;
using System;
using System.Collections;
using UnityEngine;

namespace InventoryStuff
{
    /// <summary>
    /// ScriptableObject for AOEAttackItem
    /// </summary>
    [CreateAssetMenu(
        fileName = "AOEAttackItem",
        menuName = "ScriptableObjects/Items/Special/AOEAttackItem",
        order = 0)]
    public class AOEAttackItemData : InventoryItemData
    {
        public AOEAttackItem item = new();
        public override InventoryItem GetItem() { return item; }
    }

    /// <summary>
    /// Itemtype that spawns a OnTriggerDamageEntity when used
    /// </summary>
    [Serializable]
    public class AOEAttackItem : InventoryItem
    {
        [SerializeField] float damage;
        [SerializeField] StatusEffect[] effectsApplied;
        [SerializeField] float distance = 3;
        [SerializeField] OnTriggerDamageEntity toSpawn;
        [SerializeField] float duration = .5f;

        public override void UseItem(Entity source, Vector3 inputDir)
        {
            Debug.Log($"Swung {itemName}");
            PlayerController player = source as PlayerController;
            source.StartCoroutine(UseWeapon(player));
        }

        IEnumerator UseWeapon(PlayerController source)
        {
            // Initialize
            float originalSpeed = source.moveSpeed;
            source.ChangeMoveSpeed(0);
            canUseItem = false;

            OnTriggerDamageEntity spawned = MonoBehaviour.Instantiate(toSpawn, source.transform.position + new Vector3(0, .5f, 0), source.transform.rotation);
            toSpawn.transform.localScale = new(distance, 1, distance);
            spawned.damage = damage * source.strength;
            spawned.AddException(source);

            yield return new WaitForSeconds(duration);

            // Exit
            source.ChangeMoveSpeed(originalSpeed);
            canUseItem = true;
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
