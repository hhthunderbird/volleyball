using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System;
using Unity.Netcode;
using Unity.Services.Lobbies;
using UnityEngine.Events;
using Unity.Services.Lobbies.Models;
using UnityEngine;

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

    public async UniTaskVoid CheckIfSessionExistsByName( string sessionName )
    {
        try
        {
            var query = new QueryLobbiesOptions
            {
                Filters = new List<QueryFilter>
                {
                    new(QueryFilter.FieldOptions.Name, sessionName, QueryFilter.OpOptions.EQ)
                }
            };

            var lobbyList = await LobbyService.Instance.QueryLobbiesAsync( query );

            if ( lobbyList.Results.Count > 0 )
            {
                var lobby = lobbyList.Results[ 0 ];
            }

        }
        catch ( Exception e )
        {
            Debug.LogError( e.Message );
        }

    }
}
