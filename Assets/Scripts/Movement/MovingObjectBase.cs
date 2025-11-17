using UnityEngine;

namespace MovementStuff
{
    /// <summary>
    /// Base for moving objects, useful for its baseSpeed value that can be modified by entites
    /// </summary>
    public abstract class MovingObjectBase : MonoBehaviour
    {
        public float baseSpeed = 1;
    }
}
