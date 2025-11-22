using UnityEngine;

namespace Entities
{
    public class OnLevelUpSpawnObject : EntityOnLevelUpAction
    {
        public GameObject toSpawn;
        protected override void OnLevelUp()
        {
            Instantiate(toSpawn, this.transform.position + new Vector3(0, toSpawn.transform.localScale.y, 0), 
                this.transform.rotation, this.transform);
        }
    }
}
