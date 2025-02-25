using UnityEngine;

public class BallTrajectory : MonoBehaviour
{
    private BallBehaviour _ball;
    private float _ballMass;
    private float _objectHeight = 2f; // Height of the object
    private float _gravity;

    void Start()
    {
        _ball = FindFirstObjectByType<BallBehaviour>();
        var rb = _ball.GetComponent<Rigidbody>();
        _ballMass = rb.mass;
        _gravity = Physics.gravity.y;
    }

    public void CalculateTrajectory(float angle, float force)
    {
        // Convert angle to radians
        float angleRad = angle * Mathf.Deg2Rad;

        // Calculate initial velocity components
        float v0 = force / _ballMass; // v = F/m
        float v0x = v0 * Mathf.Cos( angleRad );
        float v0y = v0 * Mathf.Sin( angleRad );

        // Calculate time of flight
        float discriminant = Mathf.Sqrt( Mathf.Pow( v0y, 2 ) + 2 * _gravity * _objectHeight);
        float timeOfFlight = ( -v0y + discriminant ) / _gravity;

        // Calculate horizontal distance
        float horizontalDistance = v0x * timeOfFlight;

        Debug.Log( "Time of Flight: " + timeOfFlight + " seconds" );
        Debug.Log( "Horizontal Distance: " + horizontalDistance + " meters" );
    }

    private Vector3 CalculateBallLandingPoint()
    {
        // Get the ball's velocity and position
        Vector3 ballVelocity = _ball.GetComponent<Rigidbody>().linearVelocity;
        Vector3 ballPosition = _ball.transform.position;

        // Calculate the angle and force of the ball's trajectory
        float angle = Mathf.Atan2( ballVelocity.y, ballVelocity.x ) * Mathf.Rad2Deg;
        float force = ballVelocity.magnitude * _ball.GetComponent<Rigidbody>().mass;

        // Use the BallTrajectory script to calculate the landing point
        _ballTrajectory.CalculateTrajectory( angle, force );

        // Estimate the landing point (replace with actual logic from BallTrajectory)
        float timeOfFlight = ( -ballVelocity.y + Mathf.Sqrt( Mathf.Pow( ballVelocity.y, 2 ) + 2 * Mathf.Abs( Physics.gravity.y ) * ballPosition.y ) ) / Mathf.Abs( Physics.gravity.y );
        Vector3 landingPoint = ballPosition + new Vector3( ballVelocity.x, 0, ballVelocity.z ) * timeOfFlight;
        landingPoint.y = 0; // Assume ground is at y = 0

        return landingPoint;
    }
}