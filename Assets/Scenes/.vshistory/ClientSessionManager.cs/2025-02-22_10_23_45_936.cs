using Unity.Netcode;
using UnityEngine.Events;

public class ClientSessionManager : NetworkBehaviour
{
    private string _sessionId;
    public string SessionId
    {
        get => _sessionId;
        set => _sessionId = value;
    }

    public UnityEvent OnGameStart;

    [Rpc( SendTo.Everyone )]
    public void StartGameRpc()
    {
        OnGameStart?.Invoke();
    }
}
