using InventoryStuff;
using System;
using System.Collections;
using UnityEngine;

public class MeleeWeaponTester : MonoBehaviour
{
    public bool showDebug = true;
    public MeleeWeaponData itemToTest;
    MeleeWeapon item => itemToTest.GetItem() as MeleeWeapon;
    private void OnValidate()
    {
        StopAllCoroutines();
        if (showDebug && itemToTest != null)
        {
            StartCoroutine(VisualDebug());
        }
    }

    IEnumerator VisualDebug()
    {
        while(showDebug)
        {
            DebugExtension.DebugCircle(this.transform.position, Color.red, item.objectDistance);
            DebugExtension.DebugCircle(this.transform.position, Color.red, item.distane);
            Debug.DrawRay(transform.position, VectorMath.RotateVectorXZ(this.transform.forward * item.distane, item.swingAngle), Color.red);
            Debug.DrawRay(transform.position, VectorMath.RotateVectorXZ(this.transform.forward * item.distane, -item.swingAngle), Color.red);
            yield return null;
        }
    }
}
