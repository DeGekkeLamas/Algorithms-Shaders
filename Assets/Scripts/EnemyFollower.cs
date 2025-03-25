using UnityEngine;

public class EnemyFollower : MonoBehaviour
{
    public float maxDistance = 10;
    public float moveSpeed = 2;
    public float battleStartDistance = 1;
    public float visionAngle = 45;

    bool hasSeenPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DebugExtension.DebugCircle(this.transform.position, Color.magenta, maxDistance);
        Debug.DrawRay(transform.position, RotateVectorXZ(transform.forward * maxDistance, visionAngle), Color.magenta);
        Debug.DrawRay(transform.position, RotateVectorXZ(transform.forward * maxDistance, -visionAngle), Color.magenta);

        // start following player if player has been seen before
        if ((PlayerController.playerReference.transform.position - this.transform.position).magnitude > .5f && (hasSeenPlayer ||
            (PlayerController.playerReference != null && 
            (PlayerController.playerReference.transform.position - this.transform.position).magnitude < maxDistance &&
            GetAngleBetweenVectors(PlayerController.playerReference.transform.position, this.transform.position) < visionAngle)))
        {
            hasSeenPlayer = true;
            this.transform.position += moveSpeed * Time.deltaTime * 
                (PlayerController.playerReference.transform.position - this.transform.position).normalized;
            transform.LookAt(PlayerController.playerReference.transform.position);
        }

        Debug.Log(GetAngleBetweenVectors(PlayerController.playerReference.transform.position, this.transform.position));
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
