using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class ClientSessionManager : NetworkBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private float _ballHeight;

    public UnityEvent OnGameStart;


    public override void OnNetworkSpawn()
    {
        if ( IsServer )
        {
            NetworkManager.Singleton.OnServerSpawnPlayer = ( player, clientId ) =>
            {
                Transform spawnPoint = GetSpawnLocation( clientId );
                player.transform.position = spawnPoint.position;
                player.transform.rotation = spawnPoint.rotation;
            };
        }
    }


    [Rpc(SendTo.ClientsAndHost)]
    public void StartGameRpc()
    {
        OnGameStart?.Invoke();
        StartGame();
    }

    public void StartGame()
    {
        if ( !IsServer ) return;

        
        Debug.Log( $"client {NetworkManager.Singleton.LocalClientId} rpc response" );
        Debug.Log( $"client {IsHost} {IsClient} {IsServer} rpc response" );
        
        
        var randomPlace = Random.insideUnitCircle * 5;
        var position = new Vector3( randomPlace.x, _ballHeight, randomPlace.y );

        var instance = Instantiate( _ballPrefab );
        instance.transform.position = position;

        var netObj = instance.GetComponent<NetworkObject>();
        netObj.Spawn();
    }
}
