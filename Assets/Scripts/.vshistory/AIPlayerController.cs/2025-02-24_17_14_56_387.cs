using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class AIPlayerController : NetworkBehaviour
{
    private NavMeshAgent _agent;
    private BallBehaviour _ball;
    public BallBehaviour Ball
    {
        get => _ball;
        set => _ball = value;
    }

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
        if ( _ball == null ) return;

        var position = _ball.BallTarget.Value;
        Gizmos.color = Color.black;
        Gizmos.DrawLine( transform.position, position );
    }
}