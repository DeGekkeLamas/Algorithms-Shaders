using InventoryStuff;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class UnitTestsInventory
{
    [UnityTest]
    public IEnumerator Items_Can_Be_Added()
    {
        yield return LoadScene("InventoryTests");
        yield return null;

        Assert.IsTrue(Inventory.instance.AddItem(new PassiveItem() ) );
    }

    [UnityTest]
    public IEnumerator Items_Cannot_Be_Added_When_Inventory_Full()
    {
        yield return LoadScene("InventoryTests");
        yield return null;
        for (int i = 0; i < Inventory.instance.currentInventory.Length; i++)
        {
            Inventory.instance.AddItem(new PassiveItem());
        }

        Assert.IsTrue(!Inventory.instance.AddItem(new PassiveItem() ) );
    }

    [UnityTest]
    public IEnumerator Inventory_Contains_Works()
    {
        yield return LoadScene("InventoryTests");
        yield return null;
        PassiveItem testItem = new();
        Inventory.instance.AddItem(testItem);

        Assert.IsTrue(Inventory.instance.Contains(testItem));
    }

    [UnityTest]
    public IEnumerator Items_Can_Be_Removed()
    {
        yield return LoadScene("InventoryTests");
        yield return null;
        PassiveItem testItem = new();
        Inventory.instance.AddItem(testItem);
        Inventory.instance.RemoveItem(testItem);

        Assert.IsTrue(!Inventory.instance.Contains(testItem));
    }

    public static IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!loadSceneOperation.isDone)
            yield return null;
    }
}
