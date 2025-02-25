using System;
using Unity.Netcode;
using UnityEngine;

public enum CourtSide
{
    Left, Right
}

public class CourtBehaviour : NetworkBehaviour
{
    [SerializeField] private CourtSide _courtSide;

    [SerializeField] private ScoreBoard _scoreBoard;
    

    private void OnCollisionEnter( Collision collision )
    {
        if ( collision.gameObject.layer == LayerMask.NameToLayer( "Ball" ) )
        {
            _scoreBoard.OnBallFallRpc( _courtSide );
            var ballRB = collision.gameObject.GetComponent<Rigidbody>();
        }
    }
}
