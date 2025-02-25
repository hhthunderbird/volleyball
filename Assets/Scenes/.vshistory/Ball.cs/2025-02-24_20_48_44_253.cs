using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 target;
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
            var angle = Vector3.Angle( Vector3.right, dir ) * Mathf.Deg2Rad;
            Debug.Log( angle * Mathf.Rad2Deg );
            dir = dir.normalized;
            rb.linearVelocity = dir * 30;
            target = Target( 30, angle );
        }
    }

    Vector3 Target( float velocity, float angle, float initialHeight )
    {
        // Get the magnitude of gravity
        float g = Mathf.Abs( Physics.gravity.y );

        // Calculate the horizontal distance (range) including initial height
        float vx = velocity * Mathf.Cos( angle ); // Horizontal velocity
        float vy = velocity * Mathf.Sin( angle ); // Vertical velocity

        // Formula for range with initial height
        float x = ( vx / g ) * ( vy + Mathf.Sqrt( Mathf.Pow( vy, 2 ) + 2 * g * initialHeight ) );

        // Return the target position relative to the ball's current position
        return transform.position + Vector3.right * x;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere( target, 1 );
    }
}
