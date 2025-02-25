using Unity.Netcode;
using UnityEngine;

public class DragAndDropManager : NetworkBehaviour
{
    [SerializeField] private PlayerInputPoll _input;
    [SerializeField] private float _thresholdDistance;

    [SerializeField] private Camera _cam;
    private Piece _currentObject;
    private bool _isBeingDragged;
    private Vector3 _difference;

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

        if ( _currentObject == null && _input.ClickToggle)
            Grab();

        if ( _currentObject != null && !_input.ClickToggle )
            Release();
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

        //Debug.Log( $"TRY HIT" );

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

            _isBeingDragged = true;
            _currentObject = piece;
            //_currentObject.Camera = _cam;
            //_currentObject.Difference = _difference;
            //_currentObject.Owner = Runner.LocalPlayer;
            //Runner.RequestStateAuthority( _currentObject.GetComponent<NetworkObject>().Id );

            //_currentObject.IsBeingDragged = true;
        }
    }

    private void Release()
    {
        _isBeingDragged = false;

        if ( _currentObject == null ) return;

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
    }


}
