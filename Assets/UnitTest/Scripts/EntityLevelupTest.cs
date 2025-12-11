using Entities;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnitTesting
{
    public class EntityLevelupTest : MonoBehaviour
    {
        [HideInInspector] public bool wasSuccess;

        private void Start()
        {
            Entity_Levelup_Is_Triggered();
        }

        void Entity_Levelup_Is_Triggered()
        {
            Entity target = this.GetComponent<Entity>();
            target.OnLevelUp += LevelUp;
            target.AddXP(target.XPRequired);
        }

        void LevelUp()
        {
            wasSuccess = true;
            Debug.Log("Levelup test success");
        }
    }
}
