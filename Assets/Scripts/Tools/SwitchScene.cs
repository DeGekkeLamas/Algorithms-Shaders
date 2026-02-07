using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple script with a function to load a scene, meant to be used in UnityEvents
/// </summary>
public class SwitchScene : MonoBehaviour
{
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
