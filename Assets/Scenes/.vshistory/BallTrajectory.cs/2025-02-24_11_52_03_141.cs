using Unity.Netcode;
using UnityEngine;

public class BallTrajectory : MonoBehaviour
{
    public GameObject Ball
    {
        set
        {
            _ballRB = value.GetComponent<Rigidbody>();
            _ballObject = value.GetComponent<NetworkObject>();
            _clientId = NetworkManager.Singleton.LocalClientId;
        }
    }

    private Rigidbody _ballRB;
    private NetworkObject _ballObject;
    private float _gravity;
    [SerializeField] private ulong _clientId;

    void Start()
    {
        _gravity = Physics.gravity.y;
    }

    public Vector3 CalculateTrajectory()
    {
        if ( _ballRB == null || _ballObject.OwnerClientId == _clientId ) return transform.position;

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