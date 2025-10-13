using UnityEngine;
using System.Collections.Generic;
public class Projectile : MonoBehaviour
{
    [HideInInspector] public float damage;
    [Header("Movement")]
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
    public static GameObject projectileContainer;

    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (useGravity) rb.useGravity = true;
        rb.AddForce(new(0, upForce, 0));
        existingProjectiles.Add(this);
        if (projectileContainer == null) projectileContainer = new("ProjectileContainer");
        this.transform.parent = projectileContainer.transform;
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
        //if (other.gameObject.TryGetComponent<Crate>(out Crate crate)) 
        //    crate.SubtractHP(1);

        // Add force to hit
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

        // Leace splat
        if (leaveSplat)
        {
            Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hitInfo);
            Instantiate(splat, hitInfo.point + new Vector3(0, 0.01f, 0), Quaternion.identity, 
                Projectile.projectileContainer.transform);
        }
    }
    private void OnDestroy() => existingProjectiles.Remove(this);
}
