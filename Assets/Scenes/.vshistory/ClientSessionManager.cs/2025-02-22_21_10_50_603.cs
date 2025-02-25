using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class ClientSessionManager : NetworkBehaviour
{
    public UnityEvent OnGameStart;

    [Rpc(SendTo.ClientsAndHost)]
    public void StartGameRpc()
    {
        Debug.Log( $"aaaaaaaaaaaaaaaaaaaaaa  {NetworkManager.Singleton.LocalClientId}" );
        //StartgameActionRpc();
    }

    [Rpc(SendTo.Everyone)]
    public void TesteClientRpc()
    {
        Debug.Log( $"ccccccccccccccccccccccc  {NetworkManager.Singleton.LocalClientId}" );
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
