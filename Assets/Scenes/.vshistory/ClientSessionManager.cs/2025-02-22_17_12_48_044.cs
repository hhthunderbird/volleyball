using Unity.Netcode;
using Unity.Services.Multiplayer;
using UnityEngine.Events;

public class ClientSessionManager : NetworkBehaviour
{
    public UnityEvent OnGameStart;

    [Rpc( SendTo.Everyone )]
    public void StartGameRpc( string sessionId )
    {
        StartGame( sessionId );
    }

    public async void StartGame(string sessionId )
    {
        try
        {
            await SessionManager.Instance.StartSession( sessionId );
            OnGameStart?.Invoke();
        }
        catch
        {

        }
    }
}
