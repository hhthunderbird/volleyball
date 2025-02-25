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
            var direction = Vector3.up + Vector3.left;
            _ballRB.AddForce( direction * _kickStrength );
        }
        if ( collision.gameObject.layer == LayerMask.NameToLayer( "Player2" ) )
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            var direction = Vector3.up + Vector3.right;
            _ballRB.AddForce( direction * _kickStrength );
        }
    }

    [Rpc( SendTo.Server )]
    private void ApplyForceServerRpc( Vector3 direction, float multiplier )
    {
        _ballRB.AddForce( direction * _kickStrength * multiplier, ForceMode.Impulse );
    }
}
