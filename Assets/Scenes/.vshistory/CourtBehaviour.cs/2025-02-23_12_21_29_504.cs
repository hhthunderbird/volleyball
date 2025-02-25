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

    public static event Action<CourtSide> OnBallFall;

    private void OnCollisionEnter( Collision collision )
    {
        if ( collision.gameObject.layer == LayerMask.NameToLayer( "Ball" ) )
            OnBallFall?.Invoke( _courtSide );
    }
}
