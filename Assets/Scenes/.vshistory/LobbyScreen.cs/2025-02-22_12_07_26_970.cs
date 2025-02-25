using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;
using Unity.Netcode;
using Unity.Services.Lobbies;
using UnityEngine;
using Unity.Services.Lobbies.Models;

public class LobbyScreen : MonoBehaviour
{
    [SerializeField] private Text _lobbyName;
    [SerializeField] private Button _confirmGameButton;
    [SerializeField] private Text _playersCountText;
    [SerializeField] private ClientSessionManager _clientSessionManager;

    public event Action OnConfirm;

    private void Awake()
    {
        _confirmGameButton.onClick.AddListener( ConfirmButton );
        NetworkManager.Singleton.OnConnectionEvent += OnConnectionEvent;
    }

    private void OnConnectionEvent( NetworkManager manager, ConnectionEventData data )
    {
        CheckIfSessionExistsByName( _lobbyName.text ).Forget();
    }

    private void ConfirmButton()
    {
        SessionManager.Instance.Startgame();

        _clientSessionManager.StartGameRpc();
    }

    private async UniTaskVoid CheckIfSessionExistsByName( string sessionName )
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
                _playersCountText.text = lobby.Players.Count.ToString();
            }

        }
        catch ( Exception e )
        {
            Debug.LogError( e.Message );
        }

    }
}
