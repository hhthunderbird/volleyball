using UnityEngine;

public class BallTrajectory : MonoBehaviour
{
    private BallBehaviour _ball;
    private Rigidbody _ballRB;
    private float _ballMass;
    private float _objectHeight = 2f; // Height of the object
    private float _gravity;

    void Start()
    {
        _ball = FindFirstObjectByType<BallBehaviour>();
        _ballRB = _ball.GetComponent<Rigidbody>();
        _ballMass = _ballRB.mass;
        _gravity = Physics.gravity.y;
    }

    public Vector3 CalculateTrajectory()
    {
        var ballVelocity = _ballRB.linearVelocity;
        var ballPosition = _ballRB.position;
        // Calculate the angle and force of the ball's trajectory
        float angle = Mathf.Atan2( ballVelocity.y, ballVelocity.x ) * Mathf.Rad2Deg;
        float force = ballVelocity.magnitude;

        // Convert angle to radians
        float angleRad = angle * Mathf.Deg2Rad;

        // Calculate initial velocity components
        float v0x = force * Mathf.Cos( angleRad );
        float v0y = force * Mathf.Sin( angleRad );

        // Calculate time of flight
        float discriminant = Mathf.Sqrt( Mathf.Pow( v0y, 2 ) + 2 * _gravity * ballPosition.y );
        float timeOfFlight = ( -v0y + discriminant ) / _gravity;

        // Calculate horizontal distance
        float horizontalDistance = v0x * timeOfFlight;

        // Calculate landing point
        Vector3 landingPoint = ballPosition + new Vector3( v0x, 0, v0y ).normalized * horizontalDistance;
        landingPoint.y = 0; // Assume ground is at y = 0

        Debug.Log( "Time of Flight: " + timeOfFlight + " seconds" );
        Debug.Log( "Horizontal Distance: " + horizontalDistance + " meters" );
        Debug.Log( "Landing Point: " + landingPoint );

        return landingPoint;
    }

}