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
            _lastMousePosition = Input.mousePosition;
            
        }
        
        if ( Input.GetMouseButtonUp( 0 ) )
            _isGrabbing = false;
        

        if ( _isGrabbing )
        {
            var currentPos = Input.mousePosition;
            var delta = currentPos - _lastMousePosition;

            var targetPos = _cam.ScreenToWorldPoint( delta );
            targetPos.z = -10;
            _cam.transform.position += targetPos;
            _lastMousePosition = currentPos;
        }
    }
}
