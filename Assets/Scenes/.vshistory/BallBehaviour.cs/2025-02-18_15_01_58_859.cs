using System;
using UnityEngine;

public enum PlayerScore
{
    Left, Right
}

public class BallBehaviour : MonoBehaviour
{
    [SerializeField] private float _kickStrength;
    private Rigidbody _ballRB;


    public event Action<PlayerScore> OnScore;

    private void Start()
    {
        _ballRB = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter( Collision collision )
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer( "Player1" ) )
        {
            var direction = Vector3.up + Vector3.left;
            _ballRB.AddForce( direction * _kickStrength );
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player2"))
        {
            var direction = Vector3.up + Vector3.right;
            _ballRB.AddForce( direction * _kickStrength );
        }

        if ( collision.gameObject.layer == LayerMask.NameToLayer( "CourtRight" ) )
            OnScore?.Invoke( PlayerScore.Left );

        if ( collision.gameObject.layer == LayerMask.NameToLayer( "CourtLeft" ) )
            OnScore?.Invoke( PlayerScore.Right );
    }
}
