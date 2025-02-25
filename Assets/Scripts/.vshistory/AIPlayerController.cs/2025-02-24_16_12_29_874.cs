using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class AIPlayerController: NetworkBehaviour
{
    private NavMeshAgent _agent;
    private Transform _target;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        if ( IsServer )
        {
            // Find the target (e.g., a player or a specific point)
            _target = GameObject.FindWithTag( "Player" ).transform;
        }
    }

    private void Update()
    {
        if ( IsServer )
        {
            // Move the AI towards the target
            if ( _target != null )
            {
                _agent.SetDestination( _target.position );
            }
        }
    }
}