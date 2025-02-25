using Unity.Netcode;
using UnityEngine;

public class DragAndDropManager : NetworkBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private PlayerInputPoll _input;
    [SerializeField] private Piece _currentObject;
    [SerializeField] private float _thresholdDistance;
    private bool _isBeingDragged;
    private Vector3 _difference;

    private Vector3 lastSentMousePosition;
    public float sendThreshold = 0.5f; // Only send if the mouse moved more than this threshold

    public void Update()
    {    
        //if ( IsOwner )
        if ( _currentObject != null && _currentObject.IsOwner )
        {
            //{
            var currentMousePosition = Input.mousePosition;

            // Check if the movement is significant enough to send an update.
            if ( Vector3.Distance( currentMousePosition, lastSentMousePosition ) > sendThreshold )
            {
                lastSentMousePosition = currentMousePosition;
                // Convert to world position if needed (example: using Camera.main.ScreenToWorldPoint)
                // Here, we assume we're sending the raw screen position.
                //SendMousePositionServerRpc( currentMousePosition );


                var screenPos = lastSentMousePosition;// Input.mousePosition;
                screenPos.z = -_cam.transform.position.z;
                var pos = _cam.ScreenToWorldPoint( screenPos ) - _difference;
                pos.z = -1f;

                _currentObject.transform.position = pos;
                Debug.Log( $"{_currentObject.transform.position}" );
            }
            //}
        }

        if ( _input.Data.Grabbed && _currentObject == null )
        {
            //Debug.Log( "GRABBED" );
            Grab();
        }

        if ( !_input.Data.Grabbed && _currentObject != null )
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
