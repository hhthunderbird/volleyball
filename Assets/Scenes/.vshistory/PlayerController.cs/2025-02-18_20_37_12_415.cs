using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpStrength;

    void Start()
    {
        
    }

    
    void Update()
    {
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
        

        if ( Input.GetKey( KeyCode.UpArrow ) )
            _player2RB.MovePosition( _player2RB.position + Vector3.forward * Time.deltaTime * _speed );
        if ( Input.GetKey( KeyCode.RightArrow) )
            _player2RB.MovePosition( _player2RB.position + Vector3.right * Time.deltaTime * _speed );
        if ( Input.GetKey( KeyCode.DownArrow ) )
            _player2RB.MovePosition( _player2RB.position + Vector3.back * Time.deltaTime * _speed );
        if ( Input.GetKey( KeyCode.LeftArrow ) )
            _player2RB.MovePosition( _player2RB.position + Vector3.left * Time.deltaTime * _speed );

        if ( Input.GetKeyUp( KeyCode.KeypadEnter) )
            _player2RB.AddForce( Vector3.up * _jumpStrength );

    }

}
