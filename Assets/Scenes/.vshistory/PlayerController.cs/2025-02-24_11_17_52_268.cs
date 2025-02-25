using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody _rigidbody;

    private BallTrajectory _ballTrajectory;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpStrength = 5f;

    private bool _isJumping;
    public bool IsJumping => _isJumping;

    private Vector3 _moveDirection;

    private float _tapThreshold = 0.3f;
    private float _inputCounter;
    private Vector3 _referencePosition;
    private bool _inputDown;

    private bool _isAiControlled;
    public bool IsAiControlled
    {
        get => _isAiControlled;
        set => _isAiControlled = value;
    }

    void Start()
    {
        _ballTrajectory = GetComponent<BallTrajectory>();
        _rigidbody = GetComponent<Rigidbody>();

        if ( !IsOwner )
            enabled = false;
    }

    void Update()
    {
        if ( !IsOwner ) return;

        if ( _isAiControlled )
        {
            var position = _ballTrajectory.CalculateTrajectory();
            var direction = position - transform.position;
            _moveDirection = new Vector3( direction.x, direction.z );
        }
        else
        {
            var pointerOverUI = EventSystem.current.IsPointerOverGameObject();

            _isJumping = _rigidbody.linearVelocity.y > 0.1f;

            if ( Input.GetMouseButtonDown( 0 ) && !pointerOverUI && !_isJumping )
            {
                _inputDown = true;
                _referencePosition = Input.mousePosition;
            }
            if ( Input.GetMouseButtonUp( 0 ) )
            {
                _inputDown = false;
                if ( _inputCounter <= _tapThreshold && !pointerOverUI )
                {
                    //tap
                    RequestJumpRpc();
                }
            }

            if ( _inputDown )
            {
                _inputCounter += Time.deltaTime;
                var direction = Input.mousePosition - _referencePosition;
                _moveDirection = new Vector3( direction.x, 0f, direction.y ).normalized * _speed;
            }
            else
            {
                _moveDirection = Vector3.zero;
            }
        }
    }

    void FixedUpdate()
    {
        if ( !IsOwner ) return;

        _rigidbody.linearVelocity = new Vector3( _moveDirection.x, _rigidbody.linearVelocity.y, _moveDirection.z );
    }

    private void OnCollisionEnter( Collision collision )
    {
        if ( !IsServer ) return;

        if ( collision.gameObject.layer == LayerMask.NameToLayer( "Ball" ) )
        {
            var netObj = collision.gameObject.GetComponent<NetworkObject>();
            RequestOwnerShipRpc(netObj, NetworkManager.Singleton.LocalClientId );
        }
    }

    [Rpc(SendTo.Server)]
    private void RequestOwnerShipRpc(NetworkObject obj, ulong id )
    {
        obj.ChangeOwnership( NetworkManager.Singleton.LocalClientId );
    }


    [Rpc( SendTo.ClientsAndHost )]
    private void RequestJumpRpc()
    {
        if ( Mathf.Abs( _rigidbody.linearVelocity.y ) < 0.1f )
            _rigidbody.AddForce( Vector3.up * _jumpStrength, ForceMode.Impulse );
    }

    private void OnDrawGizmos()
    {
        var position = _ballTrajectory.CalculateTrajectory();
        Gizmos.color = Color.black;
        Gizmos.DrawLine( transform.position, position );
    }
}
