using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class ClientSessionManager : NetworkBehaviour
{
    public UnityEvent OnGameStart;

    [Rpc( SendTo.Everyone )]
    public void StartGameRpc()
    {
        Debug.Log( "as�dflasd�flkjas�dfkla�sdfkl" );
        //StartGame( sessionId );
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
