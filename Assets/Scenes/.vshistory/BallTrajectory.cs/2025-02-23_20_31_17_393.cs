using UnityEngine;

public class BallTrajectory : MonoBehaviour
{
    private float _kickForce = 10f; // Force applied to the ball
    private float kickAngle = 45f; // Angle of kick in degrees
    private float ballMass = 1f; // Mass of the ball
    private float objectHeight = 2f; // Height of the object
    private float gravity = 9.81f; // Gravity

    void Start()
    {
        var rb = GetComponent<Rigidbody>();

        CalculateTrajectory();
    }

    void CalculateTrajectory()
    {
        // Convert angle to radians
        float angleRad = kickAngle * Mathf.Deg2Rad;

        // Calculate initial velocity components
        float v0 = _kickForce / ballMass; // v = F/m
        float v0x = v0 * Mathf.Cos( angleRad );
        float v0y = v0 * Mathf.Sin( angleRad );

        // Calculate time of flight
        float discriminant = Mathf.Sqrt( Mathf.Pow( v0y, 2 ) + 2 * gravity * objectHeight;
        float timeOfFlight = ( -v0y + discriminant ) / gravity;

        // Calculate horizontal distance
        float horizontalDistance = v0x * timeOfFlight;

        Debug.Log( "Time of Flight: " + timeOfFlight + " seconds" );
        Debug.Log( "Horizontal Distance: " + horizontalDistance + " meters" );
    }
}