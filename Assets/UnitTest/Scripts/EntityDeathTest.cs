using Entities;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnitTesting
{
    public class EntityDeathTest : MonoBehaviour
    {

        private void Start()
        {
            Entity_Death_Is_Triggered();
        }

        void Entity_Death_Is_Triggered()
        {
            Entity target = this.GetComponent<Entity>();
            target.OnDeath += Death;
            target.DealDamage(target.maxHP);
        }

        void Death()
        {
            Debug.Log("Death test success");
        }

        public static IEnumerator LoadScene(string sceneName)
        {
            AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);
            while (!loadSceneOperation.isDone)
                yield return null;
        }
    }
}
