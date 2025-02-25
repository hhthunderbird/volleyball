using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

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
            Debug.DrawLine( transform.position, target, Color.magenta );
        }
    }

    private void OnDrawGizmos()
    {
        var position = var target = _ball.BallTarget.Value;
        Gizmos.color = Color.black;
        Gizmos.DrawLine( transform.position, position );
    }
}