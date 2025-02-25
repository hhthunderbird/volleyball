using Unity.Netcode;
using UnityEngine;

public enum CourtSide
{
    Left, Right
}

public class CourtBehaviour : NetworkBehaviour
{
    private CourtSide _courtSide;
    public CourtSide CourtSide => _courtSide;

    private void OnCollisionEnter( Collision collision )
    {

    }
}
