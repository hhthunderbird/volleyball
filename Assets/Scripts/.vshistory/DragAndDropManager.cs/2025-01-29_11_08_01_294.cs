using Fusion;
using UnityEngine;

public class DragAndDropManager : NetworkBehaviour
{
    [SerializeField] private PlayerInputPoll _input;
    [SerializeField] private Piece _currentObject;
    [SerializeField] private float _thresholdDistance;
    private bool _isBeingDragged;
    private Vector3 _difference;

    public override void FixedUpdateNetwork()
    {
        ProcessInput( _input.PlayerInputData );

        if ( _isBeingDragged )
        {
            var screenPos = Input.mousePosition;
            screenPos.z = -Camera.main.transform.position.z;
            var pos = Camera.main.ScreenToWorldPoint( screenPos );

            _currentObject.Position = pos - _difference;
            //transform.SetPositionAndRotation( Position, Rotation );
        }
    }

    private void Check()
    {
        var ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        var hit = Physics2D.Raycast( ray.origin, ray.direction );

        if ( hit && hit.collider.gameObject.layer == LayerMask.NameToLayer( "piece" ) )
        {
            var piece = hit.transform.GetComponent<Piece>();

            if ( !piece.HasStateAuthority && _currentObject.Owner != Runner.LocalPlayer )
                return;

            var screenPos = Input.mousePosition;
            screenPos.z = -Camera.main.transform.position.z;
            var pos = Camera.main.ScreenToWorldPoint( screenPos );
            _difference = pos - piece.transform.position;

            _isBeingDragged = true;
            _currentObject = piece;
            _currentObject.Owner = Runner.LocalPlayer;
            _currentObject.IsBeingDragged = true;
        }
    }

    private void Drop()
    {
        _isBeingDragged = false;

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

    private void ProcessInput( PlayerInputData input )
    {
        if ( input.Pressed )
            Check();

        else if ( input.Released )
            Drop();
    }
}
