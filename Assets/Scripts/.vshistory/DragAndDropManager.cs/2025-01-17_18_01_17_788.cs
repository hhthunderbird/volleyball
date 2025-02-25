using UnityEditor.PackageManager;
using UnityEngine;

public class DragAndDropManager : MonoBehaviour
{
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        var hits = Physics2D.Raycast( ray.origin, ray.direction );

        if(hits != null)
            Debug.Log( hits.transform.gameObject.name );

    }
}
