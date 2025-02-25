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
            _currentObject.transform
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
        var hits = Physics2D.Raycast( ray.origin, ray.direction );

        if ( hits.collider.gameObject.layer == LayerMask.NameToLayer( "piece" ) )
        {
            _isDragging = true;
            _currentObject = hits.transform;
        }
            //Debug.Log( hits.transform.gameObject.name );
    }
}
