using System;
using System.Collections;
using UnityEngine;

public class WaveShaderController : MonoBehaviour
{
    Material material;
    public bool shouldDestroy = true;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }
    private void Start()
    {
        float currentTime = Time.time; //material.GetVector("_Time").y;
        material.SetFloat("_CurrentTime", currentTime);
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(material.GetFloat("_Speed")/2f);
        Destroy(this.gameObject);
    }
}
