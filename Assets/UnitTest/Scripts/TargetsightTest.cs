using Entities;
using MovementStuff;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnitTesting
{
    public class TargetsightTest : MonoBehaviour
    {
        [HideInInspector] public bool wasSuccess;

        private void Start()
        {
            Targetsight_Is_Triggered();
        }

        void Targetsight_Is_Triggered()
        {
            this.GetComponent<TargetSight>().OnFirstSeenTarget += TargetSeen;
        }

        void TargetSeen()
        {
            wasSuccess = true;
            Debug.Log("Targetsight test success");
        }
    }
}
