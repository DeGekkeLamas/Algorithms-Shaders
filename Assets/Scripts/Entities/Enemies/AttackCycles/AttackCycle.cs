using System.Collections;
using UnityEngine;

namespace Entities.Enemies
{
    public abstract class AttackCycle : MonoBehaviour
    {
        public abstract IEnumerator Attack(Enemy source);
    }
}
