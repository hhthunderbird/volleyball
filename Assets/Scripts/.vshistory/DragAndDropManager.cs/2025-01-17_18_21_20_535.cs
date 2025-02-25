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
        if ( _isDragging )
        {
            _currentObject.position = Input.mousePosition;
        }
    }

    private void OnMouseDown()
    {
        Check();
    }

    private void OnMouseUp()
    {
        _isDragging = false;
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
