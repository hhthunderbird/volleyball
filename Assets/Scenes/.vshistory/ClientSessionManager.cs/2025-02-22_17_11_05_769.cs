using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Multiplayer;
using UnityEngine.Events;

public class ClientSessionManager : NetworkBehaviour
{
    [Rpc( SendTo.Everyone )]
    public void StartGameRpc( string sessionId )
    {
        StartGame( sessionId );
    }

    public UniTask StartGame( string sessionId )
    {
        return SessionManager.Instance.StartSession( sessionId );

    }
}
