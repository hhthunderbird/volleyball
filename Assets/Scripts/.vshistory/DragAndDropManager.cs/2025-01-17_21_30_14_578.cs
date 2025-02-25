using UnityEngine;

public class DragAndDropManager : MonoBehaviour
{

    [SerializeField] private Transform _currentObject;

    [SerializeField] private bool _isDragging;

    void Start()
    {

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
            _isDragging = false;
        }

        if ( _isDragging )
        {
            var mouseP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseP.z = _currentObject.position.z;
            _currentObject.position = mouseP;
        }
    }

    private void Check()
    {
        var ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        var hit = Physics2D.Raycast( ray.origin, ray.direction );

        if ( hit.collider.gameObject.layer == LayerMask.NameToLayer( "piece" ) )
        {
            _isDragging = true;
            _currentObject = hit.transform;
        }
        //Debug.Log( hits.transform.gameObject.name );
    }
}
