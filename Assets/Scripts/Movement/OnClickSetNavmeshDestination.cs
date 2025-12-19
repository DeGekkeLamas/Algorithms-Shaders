using Entities.Player;
using UnityEngine;
using UnityEngine.AI;

namespace MovementStuff
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class OnClickSetNavmeshDestination : MonoBehaviour
    {
        NavMeshAgent navMeshAgent;

        protected void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            // Set destination on click
            if (Input.GetMouseButton(0))
            {
                SetDestination();
            }
        }

        /// <summary>
        /// Sets navmesh destination
        /// </summary>
        void SetDestination()
        {
            (bool, RaycastHit) rayHit = PlayerController.GetCamCast(LayerMask.GetMask("Terrain"));
            if (rayHit.Item1)
            {
                navMeshAgent.SetDestination(rayHit.Item2.point);
            }
        }
    }
}
