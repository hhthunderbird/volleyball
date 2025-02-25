using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ( Input.GetKeyUp( KeyCode.Space ) )
        {
            var dir = Vector3.up + Vector3.right;
            dir = dir.normalized;
            rb.linearVelocity = dir * 5;
        }
    }
}
