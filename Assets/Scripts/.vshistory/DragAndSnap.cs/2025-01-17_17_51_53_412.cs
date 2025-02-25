using UnityEngine;



public class DragAndSnap : MonoBehaviour

{

    public float gridSnapSize = 1f; // Size of your grid snap

    private Vector3 initialPosition;



    void OnMouseDown()

    {

        initialPosition = transform.position;

    }



    void OnMouseDrag()

    {

        Vector3 newPosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );

        newPosition.z = transform.position.z; // Maintain Z position

        transform.position = Vector3.SnapToGrid( newPosition, new Vector3( gridSnapSize, gridSnapSize, gridSnapSize ) );

    }

}
