using UnityEngine;
using UnityEngine.SceneManagement;
using InventoryStuff;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using Quests;
using Entities.Enemies;

namespace UnitTesting
{
    public class UnitTestsQuests
    {
        [UnityTest]
        public IEnumerator Quest_Can_Be_Added()
        {
            yield return LoadScene("InventoryTests");
            yield return null;

            FetchQuest testQuest = new();
            QuestManager.instance.AddQuest(testQuest);

            yield return null;
            yield return null;

            Assert.IsTrue(QuestManager.instance.IsActive(testQuest));
        }

        [UnityTest]
        public IEnumerator FetchQuest_Is_Completable()
        {
            yield return LoadScene("InventoryTests");
            yield return null;

            PassiveItem testItem = new();
            FetchQuest testQuest = new();
            testQuest.item = testItem;

            QuestManager.instance.AddQuest(testQuest);

            yield return null;
            yield return null;

            bool isCompleted = false;
            testQuest.OnComplete += SetToTrue;
            yield return null;
            Inventory.instance.AddItem(testItem);
            Assert.IsTrue(isCompleted);

            void SetToTrue()
            {
                isCompleted = true;
            }
        }

        [UnityTest]
        public IEnumerator SlayQuest_Is_Completable()
        {
            yield return LoadScene("InventoryTests");
            yield return null;

            GameObject entityObj = new();
            Enemy testEntity = entityObj.AddComponent<Enemy>();
            testEntity.name = "test";
            SlayQuest testQuest = new();
            testQuest.toKill = testEntity;
            testQuest.amount = 1;

            QuestManager.instance.AddQuest(testQuest);

            yield return null;
            yield return null;

            bool isCompleted = false;
            testQuest.OnComplete += SetToTrue;
            yield return null;
            testEntity.DealDamage(float.MaxValue);
            Assert.IsTrue(isCompleted);

            void SetToTrue()
            {
                isCompleted = true;
            }
        }

        public static IEnumerator LoadScene(string sceneName)
        {
            AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);
            while (!loadSceneOperation.isDone)
                yield return null;
        }
    }
}
