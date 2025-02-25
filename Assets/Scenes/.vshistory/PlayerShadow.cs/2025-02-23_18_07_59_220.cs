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
        var origin = transform.position;

        if ( Physics.Raycast( origin, Vector3.down, out var hit, 5f, _groundLayer ) )
        {
            //var newPos = hit.point + Vector3.up * _shadowOffset;
            _shadowQuad.position = hit.point;

            if ( IsServer ) // Only server syncs the position
            {
                SetShadowPositionClientRpc( hit.point );
            }
            else
            {
                RequestShadowSyncServerRpc( hit.point );
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
