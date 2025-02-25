using Unity.Netcode;
using UnityEngine;

public class PlayerShadow : NetworkBehaviour
{
    [SerializeField] private Transform _shadowQuad;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _shadowOffset = 0.05f;

    // Network variable for shadow position
    private NetworkVariable<Vector3> _shadowPosition = new NetworkVariable<Vector3>(
        Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server
    );

    private void Start()
    {
        if ( IsServer )
        {
            _shadowPosition.OnValueChanged += OnShadowPositionChanged;
        }
    }

    private void Update()
    {
        if ( IsOwner )
        {
            UpdateShadowPosition();
        }
    }

    private void UpdateShadowPosition()
    {
        var origin = transform.position;

        if ( Physics.Raycast( origin, Vector3.down, out var hit, 5f, _groundLayer ) )
        {
            var shadowPos = hit.point + Vector3.up * _shadowOffset;

            if ( IsServer )
            {
                _shadowPosition.Value = shadowPos; // Directly update on the server
            }
            else
            {
                RequestShadowUpdateMessage.SendToServer( shadowPos ); // Ask server to update
            }
        }
    }

    private void OnShadowPositionChanged( Vector3 oldPos, Vector3 newPos )
    {
        _shadowQuad.position = newPos; // Update shadow position for all clients
    }

    public struct RequestShadowUpdateMessage : INetworkMessage
    {
        public Vector3 Position;

        public void Serialize( FastBufferWriter writer )
        {
            writer.WriteValueSafe( Position );
        }

        public void Deserialize( FastBufferReader reader )
        {
            reader.ReadValueSafe( out Position );
        }

        public static void SendToServer( Vector3 position )
        {
            NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(
                nameof( RequestShadowUpdateMessage ), NetworkManager.ServerClientId, new RequestShadowUpdateMessage { Position = position }
            );
        }
    }
}
