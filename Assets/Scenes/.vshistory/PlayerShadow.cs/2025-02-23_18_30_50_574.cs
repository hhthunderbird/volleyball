using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class PlayerShadow : NetworkBehaviour
{
    [SerializeField] private Transform _shadowQuad;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _shadowOffset = 0.05f;
    [SerializeField] private float _updateInterval = 0.1f; // Adjust the update frequency

    private NetworkVariable<Vector3> _shadowPosition = new NetworkVariable<Vector3>(
        Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server
    );

    private void Start()
    {
        if ( IsServer )
        {
            StartCoroutine( ShadowUpdateRoutine() ); // Start shadow update loop
        }

        // Update shadow position whenever it changes on the network
        _shadowPosition.OnValueChanged += ( oldPos, newPos ) => _shadowQuad.position = newPos;
    }

    private IEnumerator ShadowUpdateRoutine()
    {
        while ( true ) // Keep updating the shadow indefinitely
        {
            UpdateShadowPosition();
            yield return new WaitForSeconds( _updateInterval ); // Control update frequency
        }
    }

    private void UpdateShadowPosition()
    {
        if ( Physics.Raycast( transform.position, Vector3.down, out var hit, 5f, _groundLayer ) )
        {
            var shadowPos = hit.point + Vector3.up * _shadowOffset;
            _shadowPosition.Value = shadowPos; // Server updates & syncs automatically
        }
    }
}
