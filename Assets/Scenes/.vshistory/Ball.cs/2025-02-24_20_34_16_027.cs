using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody rb;
    Vector3 target;
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
            var angle = Vector3.Angle( Vector3.up, Vector3.right ) * Mathf.Deg2Rad;
            Debug.Log( angle * Mathf.Rad2Deg );
            dir = dir.normalized;
            rb.linearVelocity = dir * 30;
            target = Target( 30, angle );
        }
    }

    Vector3 Target( float velocity, float angle )
    {
        float x = Mathf.Pow( velocity, 2 ) * Mathf.Sin( angle * 2 );
        return transform.position + Vector3.right * x;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere( target, 1 );
    }
}
