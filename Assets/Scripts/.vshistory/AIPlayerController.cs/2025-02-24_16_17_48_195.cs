using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class AIPlayerController : NetworkBehaviour
{
    private NavMeshAgent _agent;
    private BallTrajectory _ballTrajectory;

    private void Start()
    {
        _ballTrajectory = GetComponent<BallTrajectory>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if ( IsServer )
        {
            var target = _ballTrajectory.CalculateTrajectory();

            _agent.SetDestination( target );
        }

        var ballPosition = _ballTrajectory.CalculateTrajectory();
        Debug.DrawLine( transform.position, ballPosition, Color.magenta );
    }
}