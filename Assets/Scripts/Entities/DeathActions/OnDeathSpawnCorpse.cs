using UnityEngine;

namespace Entities
{
    public class OnDeathSpawnCorpse : EntityDeathAction
    {
        public Transform target;
        public GameObject corpse;
        protected override void OnDeath()
        {
            Instantiate(corpse, target.position, target.rotation);
            Destroy(target.gameObject);
        }
    }
}
