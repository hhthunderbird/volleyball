using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class ClientSessionManager : NetworkBehaviour
{
    public UnityEvent OnGameStart;

    [Rpc(SendTo.ClientsAndHost)]
    public void StartGameRpc()
    {
        OnGameStart?.Invoke();
        StartGame();
    }

    public async void StartGame()
    {
        
    }
}
