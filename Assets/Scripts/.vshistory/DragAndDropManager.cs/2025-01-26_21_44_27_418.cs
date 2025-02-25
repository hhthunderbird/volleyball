using Fusion;
using UnityEngine;

public class DragAndDropManager : NetworkBehaviour
{
    [SerializeField] private Piece _currentObject;
    [SerializeField] private float _thresholdDistance;

    [SerializeField] private bool _isDragging;
    private Vector3 _difference;

    public override void FixedUpdateNetwork()
    {
        if ( Input.touchCount == 1 || Application.isEditor )
        {
            if ( Application.isEditor )
            {
                if ( Input.GetMouseButtonDown( 0 ) )
                {
                    Check();
                }

                if ( !Input.GetMouseButton( 0 ) )
                {
                    Drop();
                }
            }
            else
            {
                var touch = Input.touches[ 0 ];

                if ( touch.phase == TouchPhase.Began )
                {
                    Check();
                }

                if ( touch.phase == TouchPhase.Ended )
                {
                    Drop();
                }

            }
        }

        //if ( _isDragging )
        //{
        //    var screenPos = Input.mousePosition;
        //    screenPos.z = -Camera.main.transform.position.z;

        //    var worldPos = Camera.main.ScreenToWorldPoint( screenPos );
        //    _difference = _currentObject.transform.position - worldPos;
        //    _difference.z = screenPos.z;

        //    _currentObject.Position = worldPos;// + _difference;
        //}
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

            _currentObject = piece;
            _currentObject.Owner = Runner.LocalPlayer;
            _currentObject.IsBeingDragged = true;
            _isDragging = true;
        }
    }

    private void Drop()
    {
        //
        if ( _currentObject == null ) return;

        var distance = Vector3.Distance( _currentObject.transform.position, _currentObject.OriginalPosition );
        var rotation = Mathf.Abs( _currentObject.OriginalRotation - _currentObject.transform.eulerAngles.z );

        if ( distance <= _thresholdDistance /*&& rotation < 5f*/ )
        {
            _currentObject.Position = _currentObject.OriginalPosition;
        }
        _currentObject.Owner = PlayerRef.None;
        _currentObject.IsBeingDragged = false;
        _isDragging = false;
        _currentObject = null;
    }
}
