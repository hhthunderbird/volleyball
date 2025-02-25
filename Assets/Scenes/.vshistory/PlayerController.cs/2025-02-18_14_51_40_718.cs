using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _player1Controller;
    [SerializeField] private CharacterController _player2Controller;

    private Rigidbody _player1RB;
    private Rigidbody _player2RB;

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpStrength;

    void Start()
    {
        _player1RB = _player1Controller.GetComponent<Rigidbody>();   
        _player2RB = _player1Controller.GetComponent<Rigidbody>();   
    }

    
    void Update()
    {
        if ( Input.GetKey( KeyCode.W ) )
            _player1RB.AddForce( Vector3.forward * Time.deltaTime);
        if ( Input.GetKey( KeyCode.A ) )
            _player1RB.AddForce( Vector3.right* Time.deltaTime);
        if ( Input.GetKey( KeyCode.S ) )
            _player1RB.AddForce( Vector3.back * Time.deltaTime);
        if ( Input.GetKey( KeyCode.D ) )
            _player1RB.AddForce( Vector3.left * Time.deltaTime);

        if (Input.GetKeyUp( KeyCode.Space ) )
            _player1RB.AddForce( Vector3.up * _jumpStrength );
        

        if ( Input.GetKey( KeyCode.UpArrow ) )
            _player2RB.AddForce( Vector3.forward * Time.deltaTime );
        if ( Input.GetKey( KeyCode.RightArrow) )
            _player2RB.AddForce( Vector3.right * Time.deltaTime );
        if ( Input.GetKey( KeyCode.DownArrow ) )
            _player2RB.AddForce( Vector3.back * Time.deltaTime );
        if ( Input.GetKey( KeyCode.LeftArrow ) )
            _player2RB.AddForce( Vector3.left * Time.deltaTime );

        if ( Input.GetKeyUp( KeyCode.KeypadEnter) )
            _player2RB.AddForce( Vector3.up * _jumpStrength );

    }

}
