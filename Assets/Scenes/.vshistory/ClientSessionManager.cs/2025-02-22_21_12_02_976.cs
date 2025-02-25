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
    }

    public async void StartGame( string sessionId )
    {
        Debug.Log( $"{IsServer}  ::: {IsLocalPlayer} ::: {IsClient} ::: {IsHost}" );

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
