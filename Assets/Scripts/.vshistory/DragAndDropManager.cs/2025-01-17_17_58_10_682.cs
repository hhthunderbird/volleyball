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
        if ( Physics2D.Raycast( Camera.main.ScreenPointToRay( Input.mousePosition ) ) )
        {
            Debug.Log( hitInfo.transform.gameObject.name );
        }
    }
}
