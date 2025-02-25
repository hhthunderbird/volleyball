using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class AIPlayerController: NetworkBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Transform target; // The target the AI will follow

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        if ( IsServer )
        {
            // Find the target (e.g., a player or a specific point)
            target = GameObject.FindWithTag( "Player" ).transform;
        }
    }

    private void Update()
    {
        if ( IsServer )
        {
            // Move the AI towards the target
            if ( target != null )
            {
                _navMeshAgent.SetDestination( target.position );
            }
        }
    }
}