using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpStrength;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        //if ( !IsOwner ) return;

        if ( Input.GetKey( KeyCode.W ) )
            _rigidbody.MovePosition( _rigidbody.position + Vector3.forward * Time.deltaTime * _speed );
        if ( Input.GetKey( KeyCode.A ) )
            _rigidbody.MovePosition( _rigidbody.position + Vector3.right* Time.deltaTime * _speed );
        if ( Input.GetKey( KeyCode.S ) )
            _rigidbody.MovePosition( _rigidbody.position + Vector3.back * Time.deltaTime * _speed );
        if ( Input.GetKey( KeyCode.D ) )
            _rigidbody.MovePosition( _rigidbody.position + Vector3.left * Time.deltaTime * _speed );

        if (Input.GetKeyUp( KeyCode.Space ) )
            _rigidbody.AddForce( Vector3.up * _jumpStrength );
    }

}
