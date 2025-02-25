using Unity.Netcode;
using UnityEngine;

public class PlayerShadow : NetworkBehaviour
{
    [SerializeField] private Transform shadowQuad;  // Reference to the quad
    [SerializeField] private LayerMask groundLayer; // Layer for the ground
    [SerializeField] private float shadowOffset = 0.05f; // Slight offset above ground

    private void Update()
    {
        if ( !IsOwner ) return; // Only the owner should update shadow position

        UpdateShadowPosition();
    }

    private void UpdateShadowPosition()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 1f; // Start ray slightly above the player

        if ( Physics.Raycast( origin, Vector3.down, out hit, 5f, groundLayer ) )
        {
            Vector3 newPos = hit.point + Vector3.up * shadowOffset; // Keep it slightly above ground
            shadowQuad.position = newPos;

            if ( IsServer ) // Only server syncs the position
            {
                SetShadowPositionClientRpc( newPos );
            }
            else
            {
                RequestShadowSyncServerRpc( newPos );
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void RequestShadowSyncServerRpc( Vector3 position )
    {
        SetShadowPositionClientRpc( position );
    }

    [Rpc( SendTo.ClientsAndHost )]
    private void SetShadowPositionClientRpc( Vector3 position )
    {
        shadowQuad.position = position;
    }
}
