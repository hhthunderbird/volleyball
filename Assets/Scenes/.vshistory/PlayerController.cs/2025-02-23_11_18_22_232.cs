using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpStrength = 5f;

    private Vector3 _moveDirection;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if ( !IsOwner ) return; 

        float moveX = 0f, moveZ = 0f;

        if ( Input.GetKey( KeyCode.W ) ) moveZ = 1f;
        if ( Input.GetKey( KeyCode.S ) ) moveZ = -1f;
        if ( Input.GetKey( KeyCode.A ) ) moveX = -1f;
        if ( Input.GetKey( KeyCode.D ) ) moveX = 1f;

        _moveDirection = new Vector3( moveX, 0f, moveZ ).normalized * _speed;

        if ( Input.GetKeyDown( KeyCode.Space ) )
        {
            RequestJumpServerRpc();
        }
    }

    void FixedUpdate()
    {
        if ( !IsOwner ) return; 

        if ( _moveDirection != Vector3.zero )
        {
            RequestMoveServerRpc( _moveDirection );
        }
    }

    [Rpc(SendTo.Server)]
    private void RequestMoveServerRpc( Vector3 moveDirection )
    {
        if ( !IsServer ) return;

        _rigidbody.MovePosition( _rigidbody.position + moveDirection * Time.fixedDeltaTime );
    }

    // 🟢 ServerRPC: Handles jumping on the server
    [Rpc( SendTo.Server )]
    private void RequestJumpServerRpc()
    {
        if ( !IsServer ) return;

        if ( Mathf.Abs( _rigidbody.velocity.y ) < 0.1f ) // Basic check if on ground
        {
            _rigidbody.AddForce( Vector3.up * _jumpStrength, ForceMode.Impulse );
        }
    }
}
