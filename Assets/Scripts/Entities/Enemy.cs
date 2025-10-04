using System.Collections;
using TMPro;
using UnityEngine;

public class Enemy : Entity
{
    public GameObject corpse;
    public GameObject tomatoSplat;

    // start battle on collision with player or projectile
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Projectile>(out Projectile hitProjectile) || other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Hit by projecile {other.gameObject.name}");
            GameManager manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            if (hitProjectile != null) Death();
        }
    }

    void Death()
    {
        GameObject oldModel = this.transform.GetChild(0).gameObject;
        Instantiate(corpse, oldModel.transform.position, oldModel.transform.rotation, this.transform);
        Destroy(oldModel);
        Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hitInfo);
        Instantiate(tomatoSplat, hitInfo.point + new Vector3(0,0.01f,0), Quaternion.identity, Projectile.projectileContainer.transform);
        StopAllCoroutines();
        if (this.TryGetComponent(out MovingObjectBase move)) Destroy(move);
        Destroy(this);
    }
}
