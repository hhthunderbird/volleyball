using Unity.Netcode;
using UnityEngine;

public class TestCameraControl : MonoBehaviour
{
    [SerializeField] private Camera _cam;

    [SerializeField] private Vector3 _centerPosition;
    [SerializeField] private Vector3 _referenceDragPoint;
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
            Debug.Log( "A" );
            if ( !_isGrabbing )
            {
                Debug.Log( "B" );
                _isGrabbing = true;

                var screenPos = Input.mousePosition;
                screenPos.z = -_cam.transform.position.z;
                var pos = _cam.ScreenToWorldPoint( screenPos );
                _referenceDragPoint = pos - transform.position;
            }

            var movement = _cam.ScreenToWorldPoint( Input.mousePosition ) - _referenceDragPoint;
            movement.z = transform.position.z;
            transform.position = movement;
        }
        else
        {
            //Debug.Log( "C" );
            _isGrabbing = false;
        }
    }

}
