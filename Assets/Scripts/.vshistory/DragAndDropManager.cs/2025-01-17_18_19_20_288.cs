using UnityEngine;

public class DragAndDropManager : MonoBehaviour
{

    [SerializeField] private GameObject _currentObject;

    [SerializeField] private bool _isDragging;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
       
    }

    private void OnMouseUp()
    {
        
    }
        

    private void Check()
    {
        var ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        var hits = Physics2D.Raycast( ray.origin, ray.direction );

        if ( hits )
            Debug.Log( hits.transform.gameObject.name );
    }
}
