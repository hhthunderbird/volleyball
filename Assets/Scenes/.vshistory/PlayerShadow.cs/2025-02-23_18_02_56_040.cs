using Unity.Netcode;
using UnityEngine;

public class PlayerShadow : NetworkBehaviour
{
    [SerializeField] private Transform _shadowQuad; 
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _shadowOffset = 0.05f; 

    private void Update()
    {
        if ( !IsOwner ) return;

        UpdateShadowPosition();
    }

    private void UpdateShadowPosition()
    {
        Vector3 origin = transform.position + Vector3.up * 1f; // Start ray slightly above the player

        if ( Physics.Raycast( origin, Vector3.down, out var hit, 5f, _groundLayer ) )
        {
            Vector3 newPos = hit.point + Vector3.up * _shadowOffset; // Keep it slightly above ground
            _shadowQuad.position = newPos;

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
        _shadowQuad.position = position;
    }
}
