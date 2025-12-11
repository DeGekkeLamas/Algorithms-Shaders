using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace UnitTesting
{
    public class UnitTestsEntity
    {
        [UnityTest]
        public IEnumerator Entity_Death_Is_Triggered()
        {
            yield return LoadScene("EntityDeathTest");

            EntityDeathTest tester = Object.FindFirstObjectByType<EntityDeathTest>();

            yield return null;
            Assert.IsTrue(tester.wasSuccess);
        }

        [UnityTest]
        public IEnumerator Entity_Levelup_Is_Triggered()
        {
            yield return LoadScene("EntityLevelupTest");

            EntityLevelupTest tester = Object.FindFirstObjectByType<EntityLevelupTest>();

            yield return null;
            Assert.IsTrue(tester.wasSuccess);
        }

        public static IEnumerator LoadScene(string sceneName)
        {
            AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);
            while (!loadSceneOperation.isDone)
                yield return null;
        }
    }
}
