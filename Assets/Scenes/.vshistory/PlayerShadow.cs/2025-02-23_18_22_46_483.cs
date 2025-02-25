using Unity.Netcode;
using UnityEngine;

public class PlayerShadow : NetworkBehaviour
{
    [SerializeField] private Transform _shadowQuad;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _shadowOffset = 0.05f;

    // Network Variable for shadow position (Server updates, Clients sync)
    private NetworkVariable<Vector3> _shadowPosition = new NetworkVariable<Vector3>(
        Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server
    );

    private void Start()
    {
        if ( IsServer )
        {
            _shadowPosition.OnValueChanged += ( oldPos, newPos ) => _shadowQuad.position = newPos;
        }
    }

    private void Update()
    {
        if ( !IsOwner ) return;

        if ( Physics.Raycast( transform.position, Vector3.down, out var hit, 5f, _groundLayer ) )
        {
            var shadowPos = hit.point + Vector3.up * _shadowOffset;

            if ( IsServer )
            {
                _shadowPosition.Value = shadowPos; // Server updates & syncs automatically
            }
            else
            {
                RequestShadowUpdateServerRpc( shadowPos ); // Ask server to update
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void RequestShadowUpdateServerRpc( Vector3 position )
    {
        _shadowPosition.Value = position;
    }
}
