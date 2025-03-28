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
    public float maxRotationPerSpin = 45;
    public float delayBetweenMovements = .5f;

    [Header("Debug")]
    public TMP_Text debugText;

    bool hasSeenPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(RandomMovements());
    }

    // Update is called once per frame
    void Update()
    {
        // shows vision range
        DebugExtension.DebugCircle(this.transform.position, Color.green, maxDistance);
        Debug.DrawRay(transform.position, RotateVectorXZ(this.transform.forward * maxDistance, 
            visionAngle), Color.green * 1.5f);
        Debug.DrawRay(transform.position, RotateVectorXZ(this.transform.forward * maxDistance, 
            -visionAngle), Color.green * 0.5f);

        // start following player if player has been seen before
        float playerEnemyDistance = (PlayerController.playerReference.transform.position - this.transform.position).magnitude;

        if (playerEnemyDistance > .5f && (hasSeenPlayer ||
            (PlayerController.playerReference != null && 
            playerEnemyDistance < maxDistance &&
            GetAngleBetweenVectors(this.transform.position - PlayerController.playerReference.transform.position, this.transform.forward) 
                < visionAngle)))
        {
            hasSeenPlayer = true;
            this.transform.position += moveSpeed * Time.deltaTime * 
                (PlayerController.playerReference.transform.position - this.transform.position).normalized;
            transform.LookAt(PlayerController.playerReference.transform.position);

            // Start battle if too close
            if (playerEnemyDistance < battleStartDistance)
            {
                GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                manager.StartBattle();
            }
        }
        debugText.text = GetAngleBetweenVectors(this.transform.position - PlayerController.playerReference.transform.position, 
            this.transform.forward).ToString();
    }

    IEnumerator RandomMovements()
    {
        while(!hasSeenPlayer)
        {
            // Rotate enemy at random
            float _rotation = Random.Range(maxRotationPerSpin * .5f, maxRotationPerSpin);
            _rotation = Random.Range(0, 2) == 0 ? 0 - _rotation : _rotation;

            Vector3 _newRotation = new(this.transform.eulerAngles.x, GetRotation(this.transform.eulerAngles.y, _rotation), this.transform.eulerAngles.z);
            Debug.Log($"new rotation is {_newRotation.y}");
            while(Mathf.RoundToInt(this.transform.eulerAngles.y) != Mathf.RoundToInt(_newRotation.y))
            {
                Debug.Log(CloserToPlusSide(this.transform.position.y, _newRotation.y));

                this.transform.eulerAngles += new Vector3(
                    0,CloserToPlusSide(this.transform.position.y, _newRotation.y) ? 
                    Mathf.Min(rotationSpeed * Time.deltaTime, 1) : -Mathf.Min(rotationSpeed * Time.deltaTime, 1), 0);
                yield return null;
            }

            Debug.Log($"Movement done, from {this}");
            yield return new WaitForSeconds(delayBetweenMovements);
        }

        // Gets rotation for spin
        static float GetRotation(float original, float rotation)
        {
            original = (original + rotation) % 360;
            if (original < 0) original = 360 + rotation;

            return original;
        }
        // Determined which direction is shorter for rotation
        static bool CloserToPlusSide(float currentRotation, float destination)
        {
            float differenceUp = (360 + destination - currentRotation) % 360;
            float differenceDown = (360 + currentRotation - destination) % 360;

            if (differenceDown < differenceUp) return false;
            else return true;
        }
    }

    static Vector3 RotateVectorXZ(Vector3 start, float rotation)
    {
        rotation *= (Mathf.PI / 180);

        return new(
                start.x * Mathf.Cos(rotation) - start.z * Mathf.Sin(rotation)
                , start.y
                , start.x * Mathf.Sin(rotation) + start.z * Mathf.Cos(rotation)
            );
    }
    static float GetAngleBetweenVectors(Vector3 start, Vector3 end)
    {
        float _angle = Mathf.Acos(Vector3.Dot(end, end - start) / (end.magnitude * (end-start).magnitude)) * (180 / Mathf.PI);
        return _angle;
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
