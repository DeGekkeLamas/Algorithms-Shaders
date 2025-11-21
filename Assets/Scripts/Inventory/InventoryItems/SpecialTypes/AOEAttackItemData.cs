using Entities.Player;
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
    public class AOEAttackItem : MeleeWeapon
    {
        public WeaponHandle toSpawn;

        protected override IEnumerator UseWeaponAnimation(PlayerController source, Vector3 inputDir)
        {
            var spawned = MonoBehaviour.Instantiate(toSpawn, source.transform.position + new Vector3(0, .5f, 0), source.transform.rotation);
            toSpawn.transform.localScale = new(distane, 1, distane);

            // Wait for object to destroy itself
            while (spawned != null) yield return null;
        }
    }
}
