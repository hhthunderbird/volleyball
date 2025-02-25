using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class ClientSessionManager : NetworkBehaviour
{
    public UnityEvent OnGameStart;

    [ServerRpc]
    public void StartGameRpc()
    {
        Debug.Log( $"aaaaaaaaaaaaaaaaaaaaaa" );
        //StartgameActionRpc();
    }

    [Rpc( SendTo.Me )]
    private void StartgameActionRpc()
    {
        Debug.Log( $"client {NetworkManager.Singleton.LocalClientId} rpc response" );
    }
    //public async void StartGame( string sessionId )
    //{
    //    Debug.Log( $"{IsServer}  ::: {IsLocalPlayer} ::: {IsClient} ::: {IsHost}" );

    //    //try
    //    //{
    //    //    await SessionManager.Instance.StartSession( sessionId );
    //    //    OnGameStart?.Invoke();
    //    //}
    //    //catch
    //    //{

    //    //}
    //}
}
