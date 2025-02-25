using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _player1Controller;
    [SerializeField] private CharacterController _player2Controller;

    [SerializeField] private float _speed;

    void Start()
    {
        
    }

    
    void Update()
    {
        if ( Input.GetKey( KeyCode.W ) )
            _player1Controller.Move( Vector3.forward * Time.deltaTime);
        if ( Input.GetKey( KeyCode.A ) )
            _player1Controller.Move( Vector3.right* Time.deltaTime);
        if ( Input.GetKey( KeyCode.S ) )
            _player1Controller.Move( Vector3.back * Time.deltaTime);
        if ( Input.GetKey( KeyCode.D ) )
            _player1Controller.Move( Vector3.left * Time.deltaTime);


        if ( Input.GetKey( KeyCode.UpArrow ) )
            _player2Controller.Move( Vector3.forward * Time.deltaTime );
        if ( Input.GetKey( KeyCode.RightArrow) )
            _player2Controller.Move( Vector3.right * Time.deltaTime );
        if ( Input.GetKey( KeyCode.DownArrow ) )
            _player2Controller.Move( Vector3.back * Time.deltaTime );
        if ( Input.GetKey( KeyCode.LeftArrow ) )
            _player2Controller.Move( Vector3.left * Time.deltaTime );

        if ( _player1Controller.isGrounded )
        {
            _player1Controller.attachedRigidbody.AddForce( Vector3.up );
        }
    }

}
