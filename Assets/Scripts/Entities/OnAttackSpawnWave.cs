using Entities.Enemies;
using UnityEngine;

/// <summary>
/// Spawns a wave when the enemy attacks, object spawned has its scale based off the attack radius
/// </summary>
public class OnAttackSpawnWave : OnAttackAction
{
    public GameObject toSpawn;
    protected override void AttackAction(float dst)
    {
        GameObject spawned = Instantiate(toSpawn, transform.position + new Vector3(0, .5f, 0), transform.rotation);
        spawned.transform.localScale = new(dst, 1, dst);
    }
}
