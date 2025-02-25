using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class AIPlayerController : NetworkBehaviour
{
    private NavMeshAgent _agent;
    private BallBehaviour _ball;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if ( IsServer )
        {
            var target = _ball.BallTarget.Value;

            _agent.SetDestination( target );
        }

        var ballPosition = _ballTrajectory.CalculateTrajectory();
        Debug.DrawLine( transform.position, ballPosition, Color.magenta );
    }

    private void OnDrawGizmos()
    {
        var position = _ballTrajectory.CalculateTrajectory();
        Gizmos.color = Color.black;
        Gizmos.DrawLine( transform.position, position );
    }
}