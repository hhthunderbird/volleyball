using Unity.Netcode;
using UnityEngine;

public class TestCameraControl : MonoBehaviour
{
    [SerializeField] private Camera _cam;

    [SerializeField] private Vector3 _centerPosition;
    [SerializeField] private Vector3 _lastMousePosition;
    [SerializeField] private float _startCameraZoom;
    [SerializeField] private bool _isGrabbing;

    private void Start()
    {
        _centerPosition = _cam.transform.position;
        _startCameraZoom = _cam.orthographicSize;
    }
    //public override void OnNetworkSpawn(){}

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetMouseButtonDown( 0 ) )
        {
            _isGrabbing = true;
            _lastMousePosition = _cam.ScreenToWorldPoint( Input.mousePosition );
            _lastMousePosition.z = -10;
        }
        
        if ( Input.GetMouseButtonUp( 0 ) )
            _isGrabbing = false;
        

        if ( _isGrabbing )
        {
            var currentPos = _cam.ScreenToWorldPoint( Input.mousePosition );
            currentPos.z = -10;
            var delta = currentPos - _lastMousePosition;
            _cam.transform.position += delta;
            _lastMousePosition = currentPos;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere( _lastMousePosition, 0.1f );
    }
}
