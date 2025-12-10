//using Entities;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using NUnit.Framework;

namespace UnitTesting
{
    public class EntityTests
    {
        bool wasSuccess;

        [UnityTest]
        IEnumerator Entity_Levelup_Is_Triggered()
        {
            //Setup the test
            yield return LoadScene("EntityLevelupTest");//Wait for the scene to finish loading. 

            //Entity target = Object.FindFirstObjectByType<Entity>();
            //target.OnLevelUp += LevelUp;
            //target.AddXP(target.XPRequired);

            //Check the result
            Assert.AreEqual(wasSuccess, true);
        }

        void LevelUp()
        {
            wasSuccess = true;
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
