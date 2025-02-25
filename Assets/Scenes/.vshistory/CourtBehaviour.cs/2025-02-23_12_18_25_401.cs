using Unity.Netcode;
using UnityEngine;

public enum CourtSide
{
    Left, Right
}

public class CourtBehaviour : NetworkBehaviour
{
    private void OnCollisionEnter( Collision collision )
    {

    }
}
