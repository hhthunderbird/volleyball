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

    private void Start()
    {
        _ballRB = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter( Collision collision )
    {
        if ( !IsServer ) return;

        if ( collision.gameObject.layer == LayerMask.NameToLayer( "Player1" ) )
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            var direction = Vector3.up + Vector3.right;
            //_ballRB.AddForce( direction * _kickStrength );

            float forceMultiplier = player.IsJumping ? _kickStrength * 2 : _kickStrength;
            //_ballRB.AddForce( direction * _kickStrength * forceMultiplier, ForceMode.Impulse );
            ApplyForceServerRpc( direction, forceMultiplier );
        }
        if ( collision.gameObject.layer == LayerMask.NameToLayer( "Player2" ) )
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            var direction = Vector3.up + Vector3.left;
            //_ballRB.AddForce( direction * _kickStrength );

            float forceMultiplier = player.IsJumping ? _kickStrength * 2 : _kickStrength;
            //_ballRB.AddForce( direction * _kickStrength * forceMultiplier, ForceMode.Impulse );
            ApplyForceServerRpc( direction, forceMultiplier );
        }
    }

    [Rpc( SendTo.Server )]
    private void ApplyForceServerRpc( Vector3 direction, float multiplier )
    {
        _ballRB.AddForce( direction * _kickStrength * multiplier, ForceMode.Impulse );
    }
}
