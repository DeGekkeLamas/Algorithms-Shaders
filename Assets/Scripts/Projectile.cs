using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 20;
    public float upForce;
    public bool useGravity;
    public Vector3 startRotationForce;

    public bool onlyDestroyOnTerrain;
    public bool destroyOnGround;
    [Header("Expansion")]
    public bool expands;
    public float expansionFactor = 1.05f;
    public float maxExpansion = 2;

    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (useGravity) rb.useGravity = true;
        rb.AddForce(new(0, upForce, 0));
        rb.AddTorque(startRotationForce);
    }
    void FixedUpdate()
    {
        if (expands && this.transform.localScale.x < maxExpansion)
            this.transform.localScale *= expansionFactor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Crate>(out Crate crate)) 
            crate.SubtractHP(1);

        if (other.gameObject.layer != 3 && !onlyDestroyOnTerrain || 
            onlyDestroyOnTerrain && other.gameObject.layer == 8 || 
            destroyOnGround && other.gameObject.layer == 3)
            Destroy(this.gameObject);
    }
}
