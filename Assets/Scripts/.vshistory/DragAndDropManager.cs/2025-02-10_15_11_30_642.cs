using Unity.Netcode;
using UnityEngine;

public enum PieceRaycastState
{
    NotFound,
    Found
}

public class DragAndDropManager : NetworkBehaviour
{
    [SerializeField] private PlayerInputPoll _input;
    [SerializeField] private float _thresholdDistance;

    [SerializeField] private Camera _cam;
    [SerializeField] private Piece _currentObject;
    private Vector3 _difference;

    [SerializeField] private PieceRaycastState _raycastState = PieceRaycastState.NotFound;

    private bool _clickState;

    //public override void OnNetworkSpawn(){}

    public void Update()
    {
        if ( !IsOwner ) return;

        if ( _currentObject != null && _currentObject.IsOwner )
        {
            var screenPos = _input.InputPosition;
            screenPos.z = -_cam.transform.position.z;
            var pos = _cam.ScreenToWorldPoint( screenPos ) - _difference;
            pos.z = -1f;

            _currentObject.transform.position = pos;
        }

        if ( _clickState != _input.ClickToggle )
        {
            _clickState = _input.ClickToggle;
            
            if ( _raycastState == PieceRaycastState.NotFound )
            {
                Grab();
            }
            else
            {
                Release();
            }

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
            var netwObj = hits[ 0 ].collider.GetComponent<NetworkObject>();

            if ( !netwObj.IsSpawned ) return;

            if ( !netwObj.IsOwner )
                netwObj.ChangeOwnership( NetworkManager.LocalClientId );

            var piece = hits[ 0 ].transform.GetComponent<Piece>();

            var screenPos = Input.mousePosition;
            screenPos.z = -_cam.transform.position.z;
            var pos = _cam.ScreenToWorldPoint( screenPos );
            _difference = pos - piece.transform.position;

            _currentObject = piece;
            _raycastState = PieceRaycastState.Found;
        }
        else
        {
            _raycastState = PieceRaycastState.NotFound;
        }
    }

    private void Release()
    {
        Debug.Log( "drop" );
        if ( _currentObject == null ) 
        { 
            _raycastState = PieceRaycastState.NotFound;
            return; 
        }

        var distance = Vector2.Distance( _currentObject.transform.position, _currentObject.OriginalPosition );
        var rotation = Mathf.Abs( _currentObject.OriginalRotation - _currentObject.transform.eulerAngles.z );

        //_currentObject.Owner = PlayerRef.None;
        //_currentObject.IsBeingDragged = false;

        if ( distance <= _thresholdDistance /*&& rotation < 5f*/ )
        {
            _currentObject.SendToOrinalPosition();
        }
        else
        {
            _currentObject.Drop();
        }

        _currentObject = null;
        _raycastState = PieceRaycastState.NotFound;
    }


}
