using Unity.Netcode;
using UnityEngine;

public enum Side
{
    Left, Right
}

public class BallBehaviour : NetworkBehaviour
{
    [SerializeField] private float _kickStrength;
    private Rigidbody _ballRB;
    private NetworkObject _ballObj;
    public NetworkVariable<Vector3> BallPosition =
        new(
            Vector3.zero,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
            );

    public NetworkVariable<Vector3> BallVelocity =
        new(
            Vector3.zero,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
            );
    public NetworkVariable<Vector3> BallTarget =
        new(
            Vector3.zero,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
            );

    [SerializeField] private Transform _courtDivisor;
    public Transform CourtDivisor
    {
        set => _courtDivisor = value;
    }

    private NetworkVariable<Vector3> KickDirection = new();

    private void Start()
    {
        _ballRB = GetComponent<Rigidbody>();
        _ballObj = GetComponent<NetworkObject>();
    }

    private void Update()
    {
        if ( IsServer )
        {
            BallVelocity.Value = _ballRB.linearVelocity;
            BallPosition.Value = transform.position;

            if ( _courtDivisor != null )
            {
                if ( transform.position.x > _courtDivisor.position.x ) // to its right
                    KickDirection.Value = Vector3.up + Vector3.left;
                else if ( transform.position.x < _courtDivisor.position.x ) // to its left
                    KickDirection.Value = Vector3.up + Vector3.right;
            }
        }

        if ( IsClient && !IsOwner )
        {
            _ballRB.linearVelocity = BallVelocity.Value;

            transform.position = Vector3.Lerp( transform.position, _ballRB.position, Time.deltaTime * 10f );
        }
    }

    private void OnCollisionEnter( Collision collision )
    {
        if ( !IsServer ) return;

        float forceMultiplier = 0;

        if ( collision.gameObject.layer == LayerMask.NameToLayer( "Player1" ) )
        {
            var playerObj = collision.gameObject.GetComponent<NetworkObject>();
            _ballObj.ChangeOwnership( playerObj.OwnerClientId );

            var player = collision.gameObject.GetComponent<PlayerController>();

            if ( player == null )
            {
                forceMultiplier = _kickStrength;
            }
            else
            {
                forceMultiplier = player.IsJumping ? _kickStrength * 2 : _kickStrength;
            }
            ApplyForceServerRpc( KickDirection.Value, forceMultiplier );

        }
        if ( collision.gameObject.layer == LayerMask.NameToLayer( "Player2" ) )
        {
            var playerObj = collision.gameObject.GetComponent<NetworkObject>();
            _ballObj.ChangeOwnership( playerObj.OwnerClientId );

            var player = collision.gameObject.GetComponent<PlayerController>();

            if ( player == null )
            {
                forceMultiplier = _kickStrength;
            }
            else
            {
                forceMultiplier = player.IsJumping ? _kickStrength + 3 : _kickStrength;
            }
            ApplyForceServerRpc( KickDirection.Value, forceMultiplier );
        }
    }

    [Rpc( SendTo.ClientsAndHost )]
    private void ApplyForceServerRpc( Vector3 direction, float multiplier )
    {
        if ( !IsServer ) return;

        direction = direction.normalized;

        var angle = 0f;

        if ( direction.x > 0 ) //right
            angle = Vector3.Angle( Vector3.right, direction ) * Mathf.Deg2Rad;
        else if ( direction.x < 0 ) // left
            angle = Vector3.Angle( Vector3.left, direction ) * Mathf.Deg2Rad;

        _ballRB.linearVelocity = direction * multiplier * _kickStrength;

        BallVelocity.Value = _ballRB.linearVelocity;

        BallTarget.Value = Target( direction, multiplier * _kickStrength, angle, transform.position.y );
    }

    private Vector3 Target( Vector3 launchDirection, float velocity, float angle, float initialHeight )
    {
        var gravity = Mathf.Abs( Physics.gravity.y );

        var xComponent = velocity * Mathf.Cos( angle );
        var yComponent = velocity * Mathf.Sin( angle );

        var xDistance = xComponent / gravity * ( yComponent + Mathf.Sqrt( Mathf.Pow( yComponent, 2 ) + ( 2 * gravity * initialHeight ) ) );

        var zDistance = xDistance * ( launchDirection.z / launchDirection.x );

        var groundTarget = transform.position + new Vector3( xDistance, 0, zDistance );

        groundTarget.y = 0;

        return groundTarget;
    }


    private void OnDrawGizmos()
    {
        if ( IsServer )
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere( BallTarget.Value, 10 );
        }
    }
}
