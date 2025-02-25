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
}