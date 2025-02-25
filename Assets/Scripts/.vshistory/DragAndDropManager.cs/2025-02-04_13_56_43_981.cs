using Fusion;
using UnityEngine;

public class DragAndDropManager : NetworkBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private PlayerInputPoll _input;
    private PlayerInputData _inputData;
    [SerializeField] private Piece _currentObject;
    [SerializeField] private float _thresholdDistance;
    private bool _isBeingDragged;
    private Vector3 _difference;

    private void Update()
    {
        ReadInput( _input.PlayerInputData );
    }

    public override void FixedUpdateNetwork()
    {
        if ( _inputData.Grabbed && _currentObject != null )
        {
            var screenPos = Input.mousePosition;
            screenPos.z = -_cam.transform.position.z;
            var pos = _cam.ScreenToWorldPoint( screenPos ) - _difference;
            pos.z = -1f;

            _currentObject.Position = pos;
        }
            

        if ( _inputData.Grabbed && _currentObject == null )
        {
            //Debug.Log( "GRABBED" );
            Grab();
        }

        if ( !_inputData.Grabbed && _currentObject != null )
        {
            //Debug.Log( "RELEASED" );
            Release();
        }
    }

    private void Grab()
    {
        var filter = new ContactFilter2D()
        {
            layerMask = LayerMask.NameToLayer( "piece" )
        };

        var hits = new RaycastHit2D[ 1 ];

        var ray = _cam.ScreenPointToRay( Input.mousePosition );

        var hitQty = Physics2D.Raycast( ray.origin, ray.direction, filter, hits );

        if ( hitQty > 0 )
        {
            var piece = hits[ 0 ].transform.GetComponent<Piece>();

            var screenPos = Input.mousePosition;
            screenPos.z = -_cam.transform.position.z;
            var pos = _cam.ScreenToWorldPoint( screenPos );
            _difference = pos - piece.transform.position;

            _isBeingDragged = true;
            _currentObject = piece;
            _currentObject.Camera = _cam;
            _currentObject.Difference = _difference;
            _currentObject.Owner = Runner.LocalPlayer;
            Runner.RequestStateAuthority( _currentObject.GetComponent<NetworkObject>().Id );
            
            _currentObject.IsBeingDragged = true;
        }
    }

    private void Release()
    {
        _isBeingDragged = false;

        if ( _currentObject == null ) return;

        var distance = Vector2.Distance( _currentObject.transform.position, _currentObject.OriginalPosition );
        var rotation = Mathf.Abs( _currentObject.OriginalRotation - _currentObject.transform.eulerAngles.z );

        _currentObject.Owner = PlayerRef.None;
        _currentObject.IsBeingDragged = false;

        if ( distance <= _thresholdDistance /*&& rotation < 5f*/ )
        {
            _currentObject.SendToOrinalPosition();
        }
        else
        {
            _currentObject.Drop();
        }

        _currentObject = null;
    }

    private void ReadInput( PlayerInputData input )
    {
        _inputData = input;
    }
}
