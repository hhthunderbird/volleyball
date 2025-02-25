using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameUI : MonoBehaviour
{

    [SerializeField] private InputField _lobbyName;
    [SerializeField] private Button _confirmButton;

    private CancellationTokenSource _cancellationTokenSource;
    private UniTask<QueryResponse> _searchRoomTask;

    private void OnEnable()
    {
        SetLobbyName( _lobbyName.text );
        _confirmButton.interactable = false;
    }

    private void Start()
    {
        _lobbyName.onValueChanged.AddListener( SetLobbyName );
        _lobbyName.text = ClientInfo.LobbyName;

    }

    private void SetLobbyName( string lobby )
    {
        _confirmButton.interactable = !string.IsNullOrEmpty( lobby );
        //ClientInfo.LobbyName = lobby;

        if ( _searchRoomTask.Status == UniTaskStatus.Pending )
            _searchRoomTask.

        _searchRoomTask = CheckIfSessionExistsByName( lobby, _cancellationTokenSource );
    }

    public UniTask CheckIfSessionExistsByName( string sessionName, CancellationToken cancellationTokenSource )
    {
        var task = new UniTask<QueryResponse>();
        try
        {
            // Query lobbies with the specified name
            var query = new QueryLobbiesOptions
            {
                Filters = new List<QueryFilter>
                {
                    new QueryFilter(QueryFilter.FieldOptions.Name, sessionName, QueryFilter.OpOptions.EQ)
                }
            };

            cancellationTokenSource.requ

            task = LobbyService.Instance.QueryLobbiesAsync( query ).AsUniTask();
            // Check if any lobby with the specified name exists
        }
        catch ( Exception e )
        {
            Debug.LogError( $"Error checking session: {e.Message}" );

        }
        return task;
    }
}
