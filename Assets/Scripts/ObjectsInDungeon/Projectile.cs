using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [HideInInspector] public float damage;
    [Header("Movement")]
    public float projectileSpeed = 20;
    public float upForce;
    public bool useGravity;
    public float rotationIntensity = 1;

    [Header("Collisions")]
    public bool destroyOnTerrain = true;
    public bool destroyOnAnything = true;
    [Header("Expansion")]
    public bool expands;
    public float expansionFactor = 1.05f;
    public float maxExpansion = 2;

    [Header("Splat")]
    public GameObject splat;
    Rigidbody rb;

    public static Transform projectileContainer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (projectileContainer == null) projectileContainer = new GameObject("ProjectileContainer").transform;
    }
    void Start()
    {
        if (useGravity) rb.useGravity = true;
        rb.AddForce(new(0, upForce, 0));
    }
    void FixedUpdate()
    {
        if (expands && this.transform.localScale.x < maxExpansion)
            this.transform.localScale *= expansionFactor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Entity entity))
        {
            entity.DealDamage(damage);
        }

        // Add force to hit
        if (other.gameObject.TryGetComponent(out Rigidbody rb))
        {
            rb.AddExplosionForce(projectileSpeed + 0.1f*upForce, this.transform.position, 5);
        }

        // destroys on terrain collision or wall collision or any collision
        if (destroyOnAnything || // Any
            other.gameObject.layer == LayerMask.GetMask("Terrain") && destroyOnTerrain || // Floor
            destroyOnTerrain && other.gameObject.layer == LayerMask.GetMask("Walls")) // Walls
        {
            Destroy(this.gameObject);
        }

        // Leave splat
        if (splat != null)
        {
            Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hitInfo);
            Instantiate(splat, hitInfo.point + new Vector3(0, 0.01f, 0), Quaternion.identity, 
                Projectile.projectileContainer.transform);
        }
    }
}
