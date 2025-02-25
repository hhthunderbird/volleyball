using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class ClientSessionManager : NetworkBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private float _ballHeight;

    public UnityEvent OnGameStart;

    [Rpc(SendTo.ClientsAndHost)]
    public void StartGameRpc()
    {
        OnGameStart?.Invoke();
        StartGame();
    }

    public async void StartGame()
    {
        //var playerInstance = Instantiate( _player );
        //var playerObj = playerInstance.GetComponent<NetworkObject>();
        //playerObj.Spawn();
        Debug.Log( $"client {NetworkManager.Singleton.LocalClientId} rpc response" );

        var randomPlace = Random.insideUnitCircle * 5;
        var position = new Vector3( randomPlace.x, _ballHeight, randomPlace.y );

        var instance = Instantiate( _ballPrefab );
        instance.transform.position = position;

        var netObj = instance.GetComponent<NetworkObject>();
        netObj.Spawn();
    }
}
