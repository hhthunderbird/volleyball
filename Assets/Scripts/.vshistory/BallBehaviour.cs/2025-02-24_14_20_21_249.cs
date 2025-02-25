using Unity.Netcode;
using UnityEngine;

public enum PlayerSide
{
    Left, Right
}

public class BallBehaviour : NetworkBehaviour
{
    [SerializeField] private float _kickStrength;
    private Rigidbody _ballRB;
    private NetworkObject _ballObj;
    public NetworkVariable<Vector3> BallPosition = new NetworkVariable<Vector3>( Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server );

    NetworkVariable<Vector3> _ballVelocity =
        new(
            Vector3.zero,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
            );

    private void Start()
    {
        _ballRB = GetComponent<Rigidbody>();
        _ballObj = GetComponent<NetworkObject>();
    }

    private void Update()
    {
        if ( IsServer )
        {
            _ballVelocity.Value = _ballRB.linearVelocity;
            BallPosition.Value = transform.position;
        }

        if ( IsClient && !IsOwner )
        {
            _ballRB.linearVelocity = _ballVelocity.Value;

            transform.position = Vector3.Lerp( transform.position, _ballRB.position, Time.deltaTime * 10f );
        }
    }

    private void OnCollisionEnter( Collision collision )
    {
        if ( !IsServer ) return;

        if ( collision.gameObject.layer == LayerMask.NameToLayer( "Player1" ) )
        {
            var player = collision.gameObject.GetComponent<PlayerController>();

            var playerObj = player.GetComponent<NetworkObject>();

            _ballObj.ChangeOwnership( playerObj.OwnerClientId );

            var direction = Vector3.up + Vector3.right * 0.5f;

            float forceMultiplier = player.IsJumping ? _kickStrength * 2 : _kickStrength;
            ApplyForceServerRpc( direction, forceMultiplier );

        }
        if ( collision.gameObject.layer == LayerMask.NameToLayer( "Player2" ) )
        {
            var player = collision.gameObject.GetComponent<PlayerController>();

            var playerObj = player.GetComponent<NetworkObject>();

            _ballObj.ChangeOwnership( playerObj.OwnerClientId );

            var direction = Vector3.up + Vector3.left * 0.5f;

            float forceMultiplier = player.IsJumping ? _kickStrength * 2 : _kickStrength;
            ApplyForceServerRpc( direction, forceMultiplier );
        }
    }

    [Rpc( SendTo.ClientsAndHost )]
    private void ApplyForceServerRpc( Vector3 direction, float multiplier )
    {
        if ( !IsServer ) return;

        _ballRB.AddForce( _kickStrength * multiplier * direction, ForceMode.Impulse );
        _ballVelocity.Value = _ballRB.linearVelocity;
    }
}
