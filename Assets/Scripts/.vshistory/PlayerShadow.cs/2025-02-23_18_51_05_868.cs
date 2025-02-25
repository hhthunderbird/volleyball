using Unity.Netcode;
using UnityEngine;

public class PlayerShadow : NetworkBehaviour
{
    [SerializeField] private Transform _shadowQuad;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _shadowOffset = 0.05f;

    private NetworkVariable<Vector3> _shadowPosition = new(
        Vector3.zero,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private void Start()
    {
        //if ( IsServer )
        _shadowPosition.OnValueChanged += ShadowOnValueChanged;
    }

    private void ShadowOnValueChanged( Vector3 previousValue, Vector3 newValue )
    {
        _shadowQuad.position = newValue;
    }

    private void Update()
    {
        if ( !IsOwner ) return;

        if ( Physics.Raycast( transform.position, Vector3.down, out var hit, 500f, _groundLayer ) )
        {
            var shadowPos = hit.point + ( Vector3.up * _shadowOffset );

            if ( IsServer )
            {
                _shadowPosition.Value = shadowPos;
            }
            else
            {
                RequestShadowUpdateServerRpc( shadowPos );
            }
        }
    }

    [Rpc( SendTo.Server )]
    private void RequestShadowUpdateServerRpc( Vector3 position )
    {
        _shadowPosition.Value = position;
    }
}
