using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameUI : MonoBehaviour
{

    [SerializeField] private InputField _lobbyName;
    [SerializeField] private Button _confirmButton;
    private Coroutine _searchRoomRoutine;

    private void OnEnable()
    {
        SetLobbyName( _lobbyName.text );

        if ( _searchRoomRoutine != null )
            StopCoroutine( _searchRoomRoutine );
        _searchRoomRoutine = StartCoroutine( SearchRoomRoutine() );
    }

    private IEnumerator SearchRoomRoutine()
    {
        NetworkManager.

        _confirmButton.interactable = false;
    }

    private void Start()
    {
        _lobbyName.onValueChanged.AddListener( SetLobbyName );
        _lobbyName.text = ClientInfo.LobbyName;
    }

    private async UniTask CheckRoomExistence()
    {
        if ( !_lobbyName.text.IsNullOrEmpty() )
        {

        }
    }

    private void SetLobbyName( string lobby )
    {
        ClientInfo.LobbyName = lobby;
        _confirmButton.interactable = !string.IsNullOrEmpty( lobby );
    }

    public async Task<bool> CheckIfSessionExistsByName( string sessionName )
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

            var lobbyList = await LobbyService.Instance.QueryLobbiesAsync( query );

            // Check if any lobby with the specified name exists
            return lobbyList.Results.Count > 0;
        }
        catch ( Exception e )
        {
            Debug.LogError( $"Error checking session: {e.Message}" );
            return false;
        }
    }
}
