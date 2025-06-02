using UnityEngine;
using System.Collections.Generic;
public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 20;
    public float upForce;
    public bool useGravity;
    public float rotationIntensity = 1;

    public bool onlyDestroyOnTerrain;
    public bool destroyOnGround;
    [Header("Expansion")]
    public bool expands;
    public float expansionFactor = 1.05f;
    public float maxExpansion = 2;

    [Header("Splat")]
    public bool leaveSplat;
    public GameObject splat;

    public static List<Projectile> existingProjectiles = new();

    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (useGravity) rb.useGravity = true;
        rb.AddForce(new(0, upForce, 0));
        existingProjectiles.Add(this);
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

        if (other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddExplosionForce(projectileSpeed + 0.1f*upForce, this.transform.position, 5);
        }

        // destroys on terrain collision or wall collision or any collision
        if (other.gameObject.layer != 3 && !onlyDestroyOnTerrain || 
            onlyDestroyOnTerrain && other.gameObject.layer == 8 || 
            destroyOnGround && other.gameObject.layer == 3)
        {
            Destroy(this.gameObject);
        }

        if (leaveSplat)
        {
            Vector3 point = other.ClosestPoint(transform.position);
            Physics.Raycast(point, point-other.transform.position, out RaycastHit hitInfo);
            Instantiate(splat, point, Quaternion.LookRotation(hitInfo.normal));
        }
    }
    private void OnDestroy() => existingProjectiles.Remove(this);
}
