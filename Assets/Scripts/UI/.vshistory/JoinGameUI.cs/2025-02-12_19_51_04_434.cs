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
using UnityEditor.Search;
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

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();

        CheckIfSessionExistsByName( lobby, _cancellationTokenSource.Token ).Forget();
    }

    public async UniTaskVoid CheckIfSessionExistsByName( string sessionName, CancellationToken cancellationTokenSource )
    {
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

            cancellationTokenSource.ThrowIfCancellationRequested();

            // Query the lobbies
            var lobbyList = await LobbyService.Instance.QueryLobbiesAsync( query);

            cancellationTokenSource.ThrowIfCancellationRequested();
            
            if(lobbyList.Results.Count > 0 )
            {
                _confirmButton.interactable = true;
            }

            // Process the results
            foreach ( var lobby in lobbyList.Results )
            {
                Debug.Log( $"Lobby Name: {lobby.Name}, Players: {lobby.Players.Count}" );
            }
        }
        catch ( Exception e )
        {
            Debug.LogError( $"Error checking session: {e.Message}" );

        }
        
    }
}
