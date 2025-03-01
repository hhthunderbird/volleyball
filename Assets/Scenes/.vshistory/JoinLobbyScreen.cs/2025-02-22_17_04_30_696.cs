using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System;
using Unity.Services.Lobbies;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;

public class JoinLobbyScreen : MonoBehaviour
{
    private string _lobbyName;
    [SerializeField] private InputField _lobbyNameInput;
    [SerializeField] private Button _searchButton;
    [SerializeField] private Button _confirmButton;
    public UnityEvent OnLobbyJoined;

    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    private void Awake()
    {
        _confirmButton.onClick.AddListener( OnConfirm );
        _searchButton.onClick.AddListener( SearchRoom );
        _lobbyNameInput.onValueChanged.AddListener( SetLobbyName );
    }

    private void OnEnable()
    {
        _searchButton.interactable = false;
        _confirmButton.interactable = false;
    }

    private void Start()
    {

    }

    private void SetLobbyName( string lobby )
    {
        _lobbyName = lobby;
        _searchButton.interactable = !string.IsNullOrEmpty( lobby );


    }

    private void SearchRoom()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = new CancellationTokenSource();

        CheckIfLobbyExistsByName( _lobbyName, _cancellationTokenSource.Token ).Forget();
    }

    private async void OnConfirm()
    {
        try
        {
            var options = new SessionOptions
            {
                Name = _lobbyName,
            }.WithDistributedAuthorityNetwork();

            Debug.Log( $"trying to join lobby name: {_lobbyName}" );

            //await MultiplayerService.Instance.JoinSessionByIdAsync( _id );
            await LobbyService.Instance.JoinLobbyByIdAsync( _lobbyName );

            Debug.Log( $"joined lobby name: {_lobbyName}" );
            OnLobbyJoined?.Invoke();
        }
        catch
        {

        }
    }

    public async UniTaskVoid CheckIfLobbyExistsByName( string sessionName, CancellationToken cancellationTokenSource )
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

            cancellationTokenSource.ThrowIfCancellationRequested();

            var lobby = await LobbyService.Instance.GetLobbyAsync( sessionName );

            Debug.Log( $"Lobby Name: {lobby.Name}, Available slots: {lobby.AvailableSlots} id {lobby.Id}" );
            
            cancellationTokenSource.ThrowIfCancellationRequested();

            _confirmButton.interactable = lobby.AvailableSlots >= 1;
        }
        catch ( Exception e )
        {
            Debug.LogError( $"Error checking session: {e.Message}" );

        }

    }
}
