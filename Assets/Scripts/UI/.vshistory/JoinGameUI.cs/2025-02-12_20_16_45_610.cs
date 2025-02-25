using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameUI : MonoBehaviour
{

    [SerializeField] private InputField _lobbyName;
    [SerializeField] private Button _confirmButton;

    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    private void Awake()
    {
        _confirmButton.onClick.AddListener( OnConfirm );
    }

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
        _cancellationTokenSource = new CancellationTokenSource();

        CheckIfSessionExistsByName( lobby, _cancellationTokenSource.Token ).Forget();
    }

    private async void OnConfirm()
    {
        try
        {
            Debug.Log( $"Session name: {_lobbyName.text}" );
            await MultiplayerService.Instance.JoinSessionByCodeAsync( _lobbyName.text );
        }
        catch
        {

        }
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
                    new(QueryFilter.FieldOptions.Name, sessionName, QueryFilter.OpOptions.EQ)
                }
            };

            cancellationTokenSource.ThrowIfCancellationRequested();

            var lobbyList = await LobbyService.Instance.QueryLobbiesAsync( query );

            cancellationTokenSource.ThrowIfCancellationRequested();

            if ( lobbyList.Results.Count > 0 )
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
