using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Playercontroller that works based off navmesh
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerControllerNavmesh : PlayerController
{
    NavMeshAgent navMeshAgent;

    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    protected override void Update()
    {
        // Set destination on click
        if (Input.GetMouseButton(0))
        {
            SetDestination();
        }

        UpdateAction(Input.GetMouseButtonDown(1), Input.GetMouseButton(1), Input.GetKeyDown(KeyCode.E));
    }

    /// <summary>
    /// Sets navmesh destination
    /// </summary>
    void SetDestination()
    {
        (bool, RaycastHit) rayHit = GetCamCast(LayerMask.GetMask("Terrain"));
        if (rayHit.Item1)
        {
            navMeshAgent.SetDestination(rayHit.Item2.point);
        }
    }
}
