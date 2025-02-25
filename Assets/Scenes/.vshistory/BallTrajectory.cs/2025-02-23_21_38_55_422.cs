using UnityEngine;

public class BallTrajectory : MonoBehaviour
{
    private BallBehaviour _ball;
    public BallBehaviour Ball
    {
        get => _ball;
        set 
        {
            _ballRB = value.GetComponent<Rigidbody>();
            _gravity = Physics.gravity.y;
            _ball = value; 
        }
    }
    private Rigidbody _ballRB;
    private float _gravity;

    void Start()
    {
        
    }

    public Vector3 CalculateTrajectory()
    {
        if ( _ball == null ) return Vector3.zero;

        var ballVelocity = _ballRB.linearVelocity;
        var ballPosition = _ballRB.position;
        
        float angle = Mathf.Atan2( ballVelocity.y, ballVelocity.x ) * Mathf.Rad2Deg;
        float force = ballVelocity.magnitude;

        float angleRad = angle * Mathf.Deg2Rad;

        float v0x = force * Mathf.Cos( angleRad );
        float v0y = force * Mathf.Sin( angleRad );

        float discriminant = Mathf.Sqrt( Mathf.Pow( v0y, 2 ) + 2 * _gravity * ballPosition.y );
        float timeOfFlight = ( -v0y + discriminant ) / _gravity;

        float horizontalDistance = v0x * timeOfFlight;

        var landingPoint = ballPosition + new Vector3( v0x, 0, v0y ).normalized * horizontalDistance;
        landingPoint.y = 0; // Assume ground is at y = 0
        return landingPoint;
    }

}