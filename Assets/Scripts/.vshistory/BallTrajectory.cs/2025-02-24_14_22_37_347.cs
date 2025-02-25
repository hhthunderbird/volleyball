using Unity.Netcode;
using UnityEngine;

public class BallTrajectory : MonoBehaviour
{
    public GameObject Ball
    {
        set
        {
            _ball = value.GetComponent<BallBehaviour>();
            _clientId = NetworkManager.Singleton.LocalClientId;
        }
    }

    private BallBehaviour _ball;
    private float _gravity;
    private ulong _clientId;

    void Start()
    {
        _gravity = Physics.gravity.y;
    }

    public Vector3 CalculateTrajectory()
    {
        if ( _ball == null || _ball.OwnerClientId == _clientId ) return transform.position;

        var ballVelocity = _ball.linearVelocity;
        var ballPosition = _ball.Ball;

        float angle = Mathf.Atan2( ballVelocity.y, ballVelocity.x ) * Mathf.Rad2Deg;
        float force = ballVelocity.magnitude;

        float angleRad = angle * Mathf.Deg2Rad;

        float v0x = force * Mathf.Cos( angleRad );
        float v0y = force * Mathf.Sin( angleRad );

        float discriminant = Mathf.Sqrt( Mathf.Pow( v0y, 2 ) + 2 * _gravity * ballPosition.y );
        float timeOfFlight = ( -v0y + discriminant ) / _gravity;

        float horizontalDistance = v0x * timeOfFlight;

        var landingPoint = ballPosition + new Vector3( v0x, 0, v0y ).normalized * horizontalDistance;
        landingPoint.y = 0; 
        return landingPoint;
    }

}