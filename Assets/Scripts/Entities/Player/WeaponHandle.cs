using UnityEngine;

[RequireComponent (typeof(Projectile))]
[RequireComponent (typeof(BoxCollider))]
public class WeaponHandle : MonoBehaviour
{
    [HideInInspector] public BoxCollider handleCollider;
    [HideInInspector] public Projectile projectile;

    private void Awake()
    {
        handleCollider = this.GetComponent<BoxCollider>();
        projectile = this.GetComponent<Projectile>();
    }
}
