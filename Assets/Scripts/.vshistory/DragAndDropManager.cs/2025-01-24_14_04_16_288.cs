using Fusion;
using UnityEngine;

public class DragAndDropManager : NetworkBehaviour
{
    private NetworkRunner _runner;
    
    [SerializeField] private Piece _currentObject;
    [SerializeField] private float _thresholdDistance;

    [SerializeField] private bool _isDragging;
    private Vector3 _difference;

    void Start()
    {
        _runner = FindFirstObjectByType<NetworkRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetMouseButtonDown( 0 ) )
        {
            Check();
        }

        if ( Input.GetMouseButtonUp( 0 ) )
        {
            Drop();
        }

        if ( _isDragging )
        {
            var screenPos = Input.mousePosition;
            screenPos.z = -Camera.main.transform.position.z;

            var worldPos = Camera.main.ScreenToWorldPoint( screenPos );
            _difference = _currentObject.transform.position - worldPos;
            _difference.z = screenPos.z;

            _currentObject.transform.position = worldPos;// + _difference;
        }
    }

    private void Check()
    {
        var ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        var hit = Physics2D.Raycast( ray.origin, ray.direction );

        if ( hit && hit.collider.gameObject.layer == LayerMask.NameToLayer( "piece" ) )
        {
            _isDragging = true;
            _currentObject = hit.transform.GetComponent<Piece>();
            _currentObject.Owner = _runner.LocalPlayer;
        }
    }

    private void Drop()
    {
        _isDragging = false;

        if ( _currentObject == null ) return;

        var distance = Vector3.Distance( _currentObject.transform.position, _currentObject.OriginalLocation );
        var rotation = _currentObject.transform.eulerAngles;

        if ( distance <= _thresholdDistance )
        {
            _currentObject.transform.position = _currentObject.OriginalLocation;
        }
        _currentObject = null;
        //_currentObject.Owner = null;
    }
}
