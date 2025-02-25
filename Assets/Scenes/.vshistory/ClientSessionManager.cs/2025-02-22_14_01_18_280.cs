using Unity.Netcode;
using UnityEngine.Events;

public class ClientSessionManager : NetworkBehaviour
{
    public UnityEvent OnGameStart;

    [Rpc( SendTo.Everyone )]
    public void StartGameRpc()
    {
        OnGameStart?.Invoke();
    }

    private void StartGame(string sessionId )
    {

    }
}
