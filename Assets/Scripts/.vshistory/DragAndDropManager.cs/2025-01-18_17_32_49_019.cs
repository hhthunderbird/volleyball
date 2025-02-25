using UnityEngine;

public class DragAndDropManager : MonoBehaviour
{

    [SerializeField] private Piece _currentObject;
    [SerializeField] private float _thresholdDistance;

    [SerializeField] private bool _isDragging;
    private Vector3 _difference;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay( Input.mousePosition ); // Convert mouse position to world space [1, 11, 12]

        var hit = Physics2D.Raycast( ray ); // Cast a ray from the mouse position [1, 12, 12]


        if ( hit )
            Debug.Log( hit.transform.gameObject.name );

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
        }
    }

    private void Drop()
    {
        _isDragging = false;

        if ( Vector3.Distance( _currentObject.transform.position, _currentObject.OriginalLocation ) <= _thresholdDistance )
        {
            _currentObject.transform.position = _currentObject.OriginalLocation;
        }
        _currentObject = null;
    }
}
