using Unity.Android.Gradle.Manifest;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum PlayerSide
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
        var direction = Vector3.zero;

        if ( collision.gameObject.layer == LayerMask.NameToLayer( "Player1" ) )
        {
            var playerObj = collision.gameObject.GetComponent<NetworkObject>();
            _ballObj.ChangeOwnership( playerObj.OwnerClientId );

            var player = collision.gameObject.GetComponent<PlayerController>();
            
            if ( player == null )
            {
                forceMultiplier = _kickStrength;
                direction = Vector3.up + Vector3.left * 0.5f;
            }
            else
            {
                forceMultiplier = player.IsJumping ? _kickStrength * 2 : _kickStrength;
                direction = Vector3.up + Vector3.left * 0.5f;
            }
            ApplyForceServerRpc( direction, forceMultiplier );

        }
        if ( collision.gameObject.layer == LayerMask.NameToLayer( "Player2" ) )
        {
            var playerObj = collision.gameObject.GetComponent<NetworkObject>();
            _ballObj.ChangeOwnership( playerObj.OwnerClientId );

            var player = collision.gameObject.GetComponent<PlayerController>();

            if ( player == null )
            {
                forceMultiplier = _kickStrength;
                direction = Vector3.up + Vector3.left * 0.5f;
            }
            else
            {
                forceMultiplier = player.IsJumping ? _kickStrength * 2 : _kickStrength;
                direction = Vector3.up + Vector3.left * 0.5f;
            }
            ApplyForceServerRpc( direction, forceMultiplier );
        }
    }

    [Rpc( SendTo.ClientsAndHost )]
    private void ApplyForceServerRpc( Vector3 direction, float multiplier )
    {
        if ( !IsServer ) return;

        _ballRB.AddForce( _kickStrength * multiplier * direction, ForceMode.Impulse );

        direction = direction.normalized;

        var angle = 0f;

        if(direction.x > 0 ) //right
            angle = Vector3.Angle( Vector3.right, direction ) * Mathf.Deg2Rad;
        else if(direction.x < 0 ) // left
            angle = Vector3.Angle( Vector3.left, direction ) * Mathf.Deg2Rad;

        _ballRB.linearVelocity = direction * multiplier * _kickStrength;

        BallVelocity.Value = _ballRB.linearVelocity;

        BallTarget.Value = Target( multiplier * _kickStrength, angle, transform.position.y );
    }

    Vector3 Target( float velocity, float angle, float initialHeight )
    {
        float g = Mathf.Abs( Physics.gravity.y );

        float vx = velocity * Mathf.Cos( angle );
        float vy = velocity * Mathf.Sin( angle );

        float x = ( vx / g ) * ( vy + Mathf.Sqrt( Mathf.Pow( vy, 2 ) + 2 * g * initialHeight ) );
        float z = ( vx / g ) * ( vy + Mathf.Sqrt( Mathf.Pow( vy, 2 ) + 2 * g * initialHeight ) );

        Vector3 groundTarget = transform.position + Vector3.right * x + Vector3.forward * z;

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
