using Entities;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnitTesting
{
    public class EntityLevelupTest : MonoBehaviour
    {
        //bool wasSuccess;

        private void Start()
        {
            Entity_Levelup_Is_Triggered();
        }

        void Entity_Levelup_Is_Triggered()
        {
            Entity target = this.GetComponent<Entity>();
            target.OnLevelUp += LevelUp;
            target.AddXP(target.XPRequired);

            //Check the result
            //yield return null;
            //Assert.AreEqual(wasSuccess, true);
        }

        void LevelUp()
        {
            //wasSuccess = true;
            Debug.Log("Levelup test success");
        }

        public static IEnumerator LoadScene(string sceneName)
        {
            AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);
            while (!loadSceneOperation.isDone)
                yield return null;
        }
    }
}
