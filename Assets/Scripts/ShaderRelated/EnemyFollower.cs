using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyFollower : MonoBehaviour
{
    public float maxDistance = 15;
    public float moveSpeed = 5;
    public float battleStartDistance = 1;
    public float visionAngle = 45;

    [Header("Movement before seeing player")]
    public float rotationSpeed = 30;
    public Vector2 RotationRange = new(30, 45);
    public float delayBetweenMovements = .5f;
    public float moveDistance = 2;

    bool hasSeenPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CloserToPlusSide(180, 100);
        StartCoroutine(RandomMovements());
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.playerReference == null) return;
        // shows vision range
        DebugExtension.DebugCircle(this.transform.position, Color.green, maxDistance);
        Debug.DrawRay(transform.position, VectorMath.RotateVectorXZ(this.transform.forward * maxDistance, visionAngle), Color.green);
        Debug.DrawRay(transform.position, VectorMath.RotateVectorXZ(this.transform.forward * maxDistance, -visionAngle), Color.green);

        Vector3 playerPos = PlayerController.playerReference.transform.position;
        float playerEnemyDistance = (playerPos - this.transform.position).magnitude;

        // start following player if player has been seen before
        Debug.DrawRay(this.transform.position + Vector3.up, playerPos + Vector3.up - (this.transform.position + transform.forward));

        if (playerEnemyDistance > .5f && playerEnemyDistance < maxDistance &&
            VectorMath.GetAngleBetweenVectors(playerPos - this.transform.position, this.transform.forward) 
                < visionAngle &&
                Physics.Raycast(this.transform.position + Vector3.up, playerPos + Vector3.up - (this.transform.position),
            out RaycastHit visionHit) && visionHit.collider.transform == PlayerController.playerReference.transform.GetChild(0))
        {
            hasSeenPlayer = true;
            Vector3 lastSeenPlayerPos = playerPos;

            // Move to player
            this.transform.position += moveSpeed * Time.deltaTime * 
                (lastSeenPlayerPos - this.transform.position).normalized;
            transform.LookAt(lastSeenPlayerPos);

            // Start battle if too close
            if (playerEnemyDistance < battleStartDistance)
            {
                GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                manager.StartBattle();
            }
        }
        else
        {
            hasSeenPlayer = false;
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
    bool coroutineIsRunning;
    IEnumerator RandomMovements()
    {
        coroutineIsRunning = true;
        while(!hasSeenPlayer)
        {
            // Rotate enemy at random
            float _rotation;
            if (Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit info, maxDistance, LayerMask.GetMask("Walls")))
            {
                _rotation = 120;
            }
            else
            {
                _rotation = Random.Range(RotationRange.x, RotationRange.y);
                _rotation = Random.Range(0, 2) == 0 ? 0 -_rotation : _rotation;
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


    // start battle on collision with player or projectile
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Projectile>(out Projectile hitProjectile) || other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Hit by projecile {other.gameObject.name}");
            GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            if (manager != null) manager.StartBattle(hitProjectile);
            else manager.StartBattle();
        }
    }
}
