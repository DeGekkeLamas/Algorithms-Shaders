using UnityEngine;
using System.Collections;

public class PatrollingMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float rotationSpeed = 30;
    public Vector2 RotationRange = new(30, 45);
    public float delayBetweenMovements = .5f;
    public float moveDistance = 2;
    [Header("Vision")]
    public float maxVisionDistance = 15;
    public float visionAngle = 45;

    public Transform target;
    bool hasSeenTarget;
    bool coroutineIsRunning;
    void Start()
    {
        StartCoroutine(RandomMovements());
    }

    void Update()
    {
        if (this.TryGetComponent(out Entity entity)) moveSpeed = entity.moveSpeed;

        if (PlayerController.instance == null) return;
        // shows vision range
        DebugExtension.DebugCircle(this.transform.position, Color.green, maxVisionDistance);
        Debug.DrawRay(transform.position, VectorMath.RotateVectorXZ(this.transform.forward * maxVisionDistance, visionAngle), Color.green);
        Debug.DrawRay(transform.position, VectorMath.RotateVectorXZ(this.transform.forward * maxVisionDistance, -visionAngle), Color.green);

        Vector3 playerPos = PlayerController.instance.transform.position;
        float playerEnemyDistance = (playerPos - this.transform.position).magnitude;

        // start following player if player has been seen before
        Debug.DrawRay(this.transform.position + Vector3.up, playerPos + Vector3.up - (this.transform.position + transform.forward));

        if (playerEnemyDistance > .5f && playerEnemyDistance < maxVisionDistance &&
            VectorMath.GetAngleBetweenVectors(playerPos - this.transform.position, this.transform.forward)
                < visionAngle &&
                Physics.Raycast(this.transform.position + Vector3.up, playerPos + Vector3.up - (this.transform.position),
            out RaycastHit visionHit) && visionHit.collider.transform == PlayerController.instance.transform.GetChild(0))
        {
            hasSeenTarget = true;
            Vector3 lastSeenPlayerPos = playerPos;

            // Move to player
            this.transform.position += moveSpeed * Time.deltaTime *
                (lastSeenPlayerPos - this.transform.position).normalized;
            transform.LookAt(lastSeenPlayerPos);
        }
        else
        {
            hasSeenTarget = false;
            if (!coroutineIsRunning) StartCoroutine(RandomMovements());
        }
        /**
        // Stop following player if object in between
        if (hasSeenPlayer && Physics.Raycast(this.transform.position, playerPos - (this.transform.position - transform.forward), 
            out RaycastHit visionHit))
            if (visionHit.collider.gameObject != PlayerController.playerReference.gameObject)
            {
                hasSeenPlayer = false;
                StopAllCoroutines();
                StartCoroutine(RandomMovements());
            }
        Debug.DrawRay(this.transform.position, playerPos - (this.transform.position + transform.forward));
        **/
    }


    IEnumerator RandomMovements()
    {
        coroutineIsRunning = true;
        while (!hasSeenTarget)
        {
            // Rotate enemy at random
            float _rotation;
            if (Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit info, maxVisionDistance, LayerMask.GetMask("Walls")))
            {
                _rotation = 120;
            }
            else
            {
                _rotation = Random.Range(RotationRange.x, RotationRange.y);
                _rotation = Random.Range(0, 2) == 0 ? 0 - _rotation : _rotation;
            }
            Vector3 _newRotation = new(this.transform.eulerAngles.x,
                GetRotation(this.transform.eulerAngles.y, _rotation), this.transform.eulerAngles.z);

            bool _rotateToPlus = CloserToPlusSide(this.transform.eulerAngles.y, _newRotation.y);
            while (Mathf.RoundToInt(this.transform.eulerAngles.y / 10) != Mathf.RoundToInt(_newRotation.y / 10))
            {
                this.transform.eulerAngles += new Vector3(
                    0, _rotateToPlus ? rotationSpeed * Time.deltaTime : -rotationSpeed * Time.deltaTime, 0);
                yield return null;
            }

            // Moves enemy forward
            Vector3 _destination = this.transform.position + this.transform.forward * moveDistance;

            while ((this.transform.position - _destination).magnitude > .25f && (this.transform.position - _destination).magnitude < 7)
            {
                this.transform.position += moveSpeed * Time.deltaTime * transform.forward;
                yield return null;

            }

            //Debug.Log($"Movement done, from {this}");
            yield return new WaitForSeconds(delayBetweenMovements);
        }
        coroutineIsRunning = false;
    }
    // Determines which direction is shorter for rotation
    static bool CloserToPlusSide(float currentRotation, float destination)
    {
        float differenceUp = (360 + destination - currentRotation) % 360;
        float differenceDown = (360 + currentRotation - destination) % 360;
        return differenceDown > differenceUp;
    }
    // Gets rotation for spin
    static float GetRotation(float original, float rotation)
    {
        original = (original + rotation) % 360;
        if (original < 0) original = 360 + rotation;

        return original;
    }
}
