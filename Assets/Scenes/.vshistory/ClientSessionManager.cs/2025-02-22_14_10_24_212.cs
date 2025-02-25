using Unity.Netcode;
using Unity.Services.Multiplayer;
using UnityEngine.Events;

public class ClientSessionManager : NetworkBehaviour
{
    public UnityEvent OnGameStart;

    [Rpc( SendTo.Everyone )]
    public void StartGameRpc( string sessionId )
    {
        OnGameStart?.Invoke();
    }

    private void StartGame(string sessionId )
    {
        SessionManager.Instance.StartSession( sessionId );
    }
}
