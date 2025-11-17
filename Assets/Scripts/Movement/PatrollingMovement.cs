using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

namespace MovementStuff
{
    public class PatrollingMovement : MovingObjectBase
    {
        [Header("Movement")]
        public float moveSpeed;
        public float rotationSpeed = 30;
        public Vector2 RotationRange = new(30, 45);
        public float delayBetweenMovements = .5f;
        public float moveDistance = 2;

        void OnEnable()
        {
            StartCoroutine(RandomMovements());
        }

        void OnDisable()
        {
            StopAllCoroutines();
        }


        IEnumerator RandomMovements()
        {
            while (true)
            {
                float _rotation;
                // ALways turn around if wall nearby
                if (Physics.Raycast(this.transform.position,
                    this.transform.forward, out _, moveDistance))
                {
                    _rotation = 120;
                }
                else
                {
                    // Get rotation if no wall in front
                    _rotation = Random.Range(RotationRange.x, RotationRange.y);
                    // Negative or positive
                    _rotation = Random.Range(0, 2) == 0 ? -_rotation : _rotation;
                }
                Vector3 _newRotation = new(this.transform.eulerAngles.x,
                    AddRotation(this.transform.eulerAngles.y, _rotation), this.transform.eulerAngles.z);

                bool _rotateToPlus = CloserToPlusSide(this.transform.eulerAngles.y, _newRotation.y);

                // Do rotation
                _rotation = Mathf.Abs(_rotation);
                for (float i = 0; i < _rotation; i += rotationSpeed * Time.deltaTime)
                {
                    this.transform.eulerAngles += new Vector3(
                        0, _rotateToPlus ? rotationSpeed * Time.deltaTime : -rotationSpeed * Time.deltaTime, 0);
                    yield return null;
                }

                // Moves enemy forward
                Vector3 _destination = this.transform.position + this.transform.forward * moveDistance;
                float dstToDestination = (this.transform.position - _destination).magnitude;

                for (float i = 0; i < dstToDestination; i += baseSpeed * moveSpeed * Time.deltaTime)
                {
                    this.transform.position += baseSpeed * moveSpeed * Time.deltaTime * transform.forward;
                    yield return null;
                }

                yield return new WaitForSeconds(delayBetweenMovements);
            }
        }

        /// <summary>
        /// Determines which direction is shorter for rotation
        /// </summary>
        static bool CloserToPlusSide(float currentRotation, float destination)
        {
            float differenceUp = (360 + destination - currentRotation) % 360;
            float differenceDown = (360 + currentRotation - destination) % 360;
            return differenceDown > differenceUp;
        }

        /// <summary>
        /// Gets rotation for spin
        /// </summary>
        static float AddRotation(float original, float rotation)
        {
            original = (original + rotation) % 360;
            if (original < 0) original = 360 + rotation;

            return original;
        }
    }
}
