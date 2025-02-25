using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class ClientSessionManager : NetworkBehaviour
{
    public UnityEvent OnGameStart;

    [ServerRpc(RequireOwnership = false)]
    public void TesteRpc()
    {
        Debug.Log( $"aaaaaaaaaaaaaaaaaaaaaa" );
        //StartgameActionRpc();
    }

    [ClientRpc(RequireOwnership = false)]
    public void BRpc()
    {
        Debug.Log( $"ccccccccccccccccccccccc" );
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
