using Fusion;
using UnityEngine;

public class DragAndDropManager : NetworkBehaviour
{
    [SerializeField] private Piece _currentObject;
    [SerializeField] private float _thresholdDistance;

    [SerializeField] private bool _isDragging;
    private Vector3 _difference;

    void Start()
    {
        
    }

    public override void FixedUpdateNetwork()
    {
        if ( Input.GetMouseButtonDown( 0 ) )
        {
            if ( _isDragging )
                Drop();
            else
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
            _currentObject = hit.transform.GetComponent<Piece>();

            if ( !_currentObject.HasStateAuthority && _currentObject.Owner != Runner.LocalPlayer ) 
            { 
                _currentObject = null;
                return; 
            }

            _currentObject.Owner = Runner.LocalPlayer;

            _isDragging = true;
        }
    }

    private void Drop()
    {
        Debug.Log( "A" );
        if ( _currentObject == null ) return;

        var distance = Vector3.Distance( _currentObject.transform.position, _currentObject.OriginalLocation );
        var rotation = Mathf.Abs( _currentObject.OriginalRotation - _currentObject.transform.eulerAngles.z);

        if ( distance <= _thresholdDistance /*&& rotation < 5f*/ )
        {
            _currentObject.transform.position = _currentObject.OriginalLocation;
        }
        _currentObject.Owner = PlayerRef.None;
        _isDragging = false;
        _currentObject = null;
        Debug.Log( "B" );
    }
}
