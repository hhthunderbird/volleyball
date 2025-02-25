using Unity.Netcode;
using UnityEngine;

public class TestCameraControl : MonoBehaviour
{
    [SerializeField] private Camera _cam;

    private Vector3 _centerPosition;
    private Vector3 _referenceDragPoint;
    private float _startCameraZoom;

    public float speed = 20;

    private bool _isGrabbing;

    //private ClientNetworkTransform _transform;

    private void Start()
    {
        //_transform = GetComponent<ClientNetworkTransform>();
        _centerPosition = _cam.transform.position;
        _startCameraZoom = _cam.orthographicSize;
    }
    //public override void OnNetworkSpawn(){}

    // Update is called once per frame
    void Update()
    {
        if ( _input.Grabbing && !_isGrabbing )
        {
            _isGrabbing = true;
            var screenPos = Input.mousePosition;
            screenPos.z = -_cam.transform.position.z;
            var pos = _cam.ScreenToWorldPoint( screenPos );
            _referenceDragPoint = pos - transform.position;
        }

        if ( _input.IsMoving && _input.Grabbing && _isGrabbing )
        {
            //var movement = new Vector3( Input.GetAxis( "Horizontal" ), Input.GetAxis( "Vertical" ) );
            //_transform.transform.position += movement * speed * Time.deltaTime;
            var movement = _cam.ScreenToWorldPoint( Input.mousePosition ) - _referenceDragPoint;
            movement.z = transform.position.z;
            transform.position = movement;
        }

        if ( !_input.Grabbing && _isGrabbing )
        {
            _isGrabbing = false;

        }
    }

    private void LateUpdate()
    {
        if ( !IsOwner ) return;

    }
}
