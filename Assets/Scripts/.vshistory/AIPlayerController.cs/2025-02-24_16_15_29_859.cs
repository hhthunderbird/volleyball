using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class AIPlayerController: NetworkBehaviour
{
    private NavMeshAgent _agent;
    private Transform _target;
    private BallTrajectory _ballTrajectory;

    private void Start()
    {
        _ballTrajectory = GetComponent<BallTrajectory>();
        _agent = GetComponent<NavMeshAgent>();

        if ( IsServer )
            _target = GameObject.FindWithTag( "Player" ).transform;
    }

    private void Update()
    {
        if ( IsServer )
        {
            var position = _ballTrajectory.CalculateTrajectory();

            if ( _target != null )
                _agent.SetDestination( _target.position );
        }
    }
}