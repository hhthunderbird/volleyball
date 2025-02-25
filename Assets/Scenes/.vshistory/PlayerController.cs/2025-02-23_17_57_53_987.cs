using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpStrength = 5f;
    private bool _isJumping;
    public bool IsJumping => _isJumping;

    private Vector3 _moveDirection;

    [SerializeField] private Transform shadowQuad;  // Reference to the quad
    [SerializeField] private LayerMask groundLayer; // Layer for the ground
    [SerializeField] private float shadowOffset = 0.05f; // Slight offset above ground

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        // Ensure only the owner can control their movement
        if ( !IsOwner )
        {
            enabled = false; // Disable the script for non-owners (let ClientNetworkTransform handle movement sync)
        }
    }

    void Update()
    {
        if ( !IsOwner ) return; // Ensure only the local player can control movement

        _isJumping = _rigidbody.linearVelocity.y > 0.1f;

        float moveX = 0f, moveZ = 0f;

        if ( Input.GetKey( KeyCode.W ) ) moveZ = 1f;
        if ( Input.GetKey( KeyCode.S ) ) moveZ = -1f;
        if ( Input.GetKey( KeyCode.A ) ) moveX = -1f;
        if ( Input.GetKey( KeyCode.D ) ) moveX = 1f;

        _moveDirection = new Vector3( moveX, 0f, moveZ ).normalized * _speed;

        if ( Input.GetKeyDown( KeyCode.Space ) )
        {
            RequestJumpRpc();
        }
    }

    void FixedUpdate()
    {
        if ( !IsOwner ) return;

        // Move using Rigidbody physics (Client-side movement)
        _rigidbody.linearVelocity = new Vector3( _moveDirection.x, _rigidbody.linearVelocity.y, _moveDirection.z );
    }

    [Rpc( SendTo.ClientsAndHost )]
    private void RequestJumpRpc()
    {
        if ( Mathf.Abs( _rigidbody.linearVelocity.y ) < 0.1f ) // Check if on the ground
        {
            _rigidbody.AddForce( Vector3.up * _jumpStrength, ForceMode.Impulse );
        }
    }
}
