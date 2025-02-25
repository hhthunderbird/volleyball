using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class ClientSessionManager : NetworkBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _ballHeight;
    [SerializeField] private Transform _leftSideSpawnPoint;
    [SerializeField] private Transform _rightSideSpawnPoint;

    public UnityEvent OnGameStart;

    [Rpc(SendTo.ClientsAndHost)]
    public void StartGameRpc()
    {
        OnGameStart?.Invoke();
        if ( IsServer )
        {
        var randomPlace = Random.insideUnitCircle * 5;
        var position = new Vector3( randomPlace.x, _ballHeight, randomPlace.y );

        var instance = Instantiate( _ballPrefab );
        instance.transform.position = (_leftSideSpawnPoint.position + _rightSideSpawnPoint.position) / 2;// position;

        var netObj = instance.GetComponent<NetworkObject>();
        netObj.Spawn();

            SpawnPlayerRpc();
        }
    }

    [Rpc(SendTo.Server)]
    public void SpawnPlayerRpc()
    {
        //Debug.Log( $"client {NetworkManager.Singleton.LocalClientId} rpc response" );
        //Debug.Log( $"client {IsHost} {IsClient} {IsServer} rpc response" );

        var playerInstance = Instantiate( _player );
        playerInstance.transform.position = _leftSideSpawnPoint.position;
        var playerObj = playerInstance.GetComponent<NetworkObject>();
        playerObj.Spawn();

    }
}
