using Entities.Enemies;
using UnityEngine;

public class OnAttackSpawnWave : OnAttackAction
{
    public GameObject toSpawn;
    protected override void AttackAction(float dst)
    {
        Instantiate(toSpawn, transform.position + new Vector3(0, .5f, 0), transform.rotation);
        toSpawn.transform.localScale = new(dst, 1, dst);
    }
}
