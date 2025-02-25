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

    [Rpc( SendTo.ClientsAndHost )]
    public void StartGameRpc()
    {
        OnGameStart?.Invoke();
        if ( IsServer )
        {
            SpawnPlayers();

            Invoke( nameof( SpawnBall ), 3 );
        }
    }

    private void SpawnPlayers()
    {
        foreach ( var client in NetworkManager.Singleton.ConnectedClientsList )
        {
            var spawnPoint = client.ClientId % 2 == 0 ? _leftSideSpawnPoint : _rightSideSpawnPoint;

            GameObject playerInstance = Instantiate( _player, spawnPoint.position, Quaternion.identity );
            NetworkObject netObj = playerInstance.GetComponent<NetworkObject>();

            netObj.SpawnWithOwnership( client.ClientId );
        }
    }

    private void SpawnBall()
    {
        var randomPlace = Random.insideUnitCircle * 5;
        var position = new Vector3( randomPlace.x, _ballHeight, randomPlace.y );

        var instance = Instantiate( _ballPrefab );
        instance.transform.position = ( _leftSideSpawnPoint.position + _rightSideSpawnPoint.position ) / 2;// position;

        var netObj = instance.GetComponent<NetworkObject>();
        netObj.Spawn();
    }
}
