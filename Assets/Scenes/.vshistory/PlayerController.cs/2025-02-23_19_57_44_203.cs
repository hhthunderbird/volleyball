//using Unity.Netcode;
//using UnityEngine;

//public class PlayerController : NetworkBehaviour
//{
//    private Rigidbody _rigidbody;

//    [SerializeField] private float _speed = 5f;
//    [SerializeField] private float _jumpStrength = 5f;

//    private bool _isJumping;
//    public bool IsJumping => _isJumping;

//    private Vector3 _moveDirection;

//    void Start()
//    {
//        _rigidbody = GetComponent<Rigidbody>();

//        if ( !IsOwner )
//            enabled = false; 
//    }

//    void Update()
//    {
//        if ( !IsOwner ) return;

//        _isJumping = _rigidbody.linearVelocity.y > 0.1f;

//        float moveX = 0f, moveZ = 0f;

//        if ( Input.GetKey( KeyCode.W ) ) moveZ = 1f;
//        if ( Input.GetKey( KeyCode.S ) ) moveZ = -1f;
//        if ( Input.GetKey( KeyCode.A ) ) moveX = -1f;
//        if ( Input.GetKey( KeyCode.D ) ) moveX = 1f;

//        _moveDirection = new Vector3( moveX, 0f, moveZ ).normalized * _speed;

//        if ( Input.GetKeyDown( KeyCode.Space ) )
//            RequestJumpRpc();
//    }

//    void FixedUpdate()
//    {
//        if ( !IsOwner ) return;

//        _rigidbody.linearVelocity = new Vector3( _moveDirection.x, _rigidbody.linearVelocity.y, _moveDirection.z );
//    }

//    [Rpc( SendTo.ClientsAndHost )]
//    private void RequestJumpRpc()
//    {
//        if ( Mathf.Abs( _rigidbody.linearVelocity.y ) < 0.1f ) 
//            _rigidbody.AddForce( Vector3.up * _jumpStrength, ForceMode.Impulse );
//    }
//}


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

    private float _tapThreshold = 0.3f;
    private float _inputCounter;
    private Vector3 _referencePosition;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        if ( !IsOwner )
            enabled = false;
    }

    void Update()
    {
        if ( !IsOwner ) return;

        _isJumping = _rigidbody.linearVelocity.y > 0.1f;

        float moveX = 0f, moveZ = 0f;

        if ( Input.GetKey( KeyCode.W ) ) moveZ = 1f;
        if ( Input.GetKey( KeyCode.S ) ) moveZ = -1f;
        if ( Input.GetKey( KeyCode.A ) ) moveX = -1f;
        if ( Input.GetKey( KeyCode.D ) ) moveX = 1f;

        _moveDirection = new Vector3( moveX, 0f, moveZ ).normalized * _speed;

        if ( Input.GetKeyDown( KeyCode.Space ) )
            RequestJumpRpc();



        if ( Input.GetMouseButtonDown( 0 ) )
        {
            _inputCounter += Time.deltaTime;
            _referencePosition = Input.mousePosition;
        }
        if ( Input.GetMouseButtonUp( 0 ) )
        {
            if(_inputCounter <= _tapThreshold )
            {
                //tap
            }
        }
    }

    void FixedUpdate()
    {
        if ( !IsOwner ) return;

        _rigidbody.linearVelocity = new Vector3( _moveDirection.x, _rigidbody.linearVelocity.y, _moveDirection.z );
    }

    [Rpc( SendTo.ClientsAndHost )]
    private void RequestJumpRpc()
    {
        if ( Mathf.Abs( _rigidbody.linearVelocity.y ) < 0.1f )
            _rigidbody.AddForce( Vector3.up * _jumpStrength, ForceMode.Impulse );
    }
}
