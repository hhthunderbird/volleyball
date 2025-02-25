using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _player1Controller;
    [SerializeField] private CharacterController _player2Controller;

    void Start()
    {
        
    }

    
    void Update()
    {
        if ( Input.GetKey( KeyCode.W ) )
        {
            _player1Controller.Move( Vector3.forward * Time.deltaTime);
        }
    }
}
