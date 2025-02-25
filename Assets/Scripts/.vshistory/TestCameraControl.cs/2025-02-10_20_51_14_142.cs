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
            _isGrabbing = true;

            var pos = _cam.ScreenToWorldPoint( Input.mousePosition );
            pos.z = -10;
            _referenceDragPoint = pos - transform.position;

        }
        if ( Input.GetMouseButtonUp( 0 ) )
        {
            _isGrabbing = false;
        }

        if ( _isGrabbing )
        {
            var pos = _cam.ScreenToWorldPoint( Input.mousePosition );
            pos.z = -10;
            var movement = pos - _referenceDragPoint;
            transform.position = movement;
        }
    }

}
