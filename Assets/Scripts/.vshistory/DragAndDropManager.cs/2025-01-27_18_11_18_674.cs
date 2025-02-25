using Fusion;
using UnityEngine;

public class DragAndDropManager : NetworkBehaviour
{
    [SerializeField] private Piece _currentObject;
    [SerializeField] private float _thresholdDistance;

    public override void FixedUpdateNetwork()
    {
        if ( GetInput<NetworkInputData>( out var input ) )
        {
            Debug.Log( "i" );
            if ( input.InputData.IsSet(Inputs.Pressed))
                Check();

            if ( input.InputData.IsSet( Inputs.Released) )
                Drop();
        }
    }

    private void Check()
    {
        Debug.Log( "Drag" );
        var ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        var hit = Physics2D.Raycast( ray.origin, ray.direction );

        if ( hit && hit.collider.gameObject.layer == LayerMask.NameToLayer( "piece" ) )
        {
            var piece = hit.transform.GetComponent<Piece>();

            if ( !piece.HasStateAuthority && _currentObject.Owner != Runner.LocalPlayer )
                return;

            _currentObject = piece;
            _currentObject.Owner = Runner.LocalPlayer;
            _currentObject.IsBeingDragged = true;
        }
    }

    private void Drop()
    {
        Debug.Log( "Drop" );
        if ( _currentObject == null ) return;

        var distance = Vector3.Distance( _currentObject.transform.position, _currentObject.OriginalPosition );
        var rotation = Mathf.Abs( _currentObject.OriginalRotation - _currentObject.transform.eulerAngles.z );

        if ( distance <= _thresholdDistance /*&& rotation < 5f*/ )
        {
            _currentObject.Position = _currentObject.OriginalPosition;
        }
        _currentObject.Owner = PlayerRef.None;
        _currentObject.IsBeingDragged = false;
        _currentObject = null;
    }
}
