using System;
using Unity.Services.Lobbies;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : Singleton<LobbyManager>
{
    public event Action OnLobbyStarted;


    public async void StartLobby()
    {
        var sessionName = PlayerPrefs.GetString( PlayerPrefsKeys.RoomName.ToString() );
        var playersLimit = PlayerPrefs.GetInt( PlayerPrefsKeys.RoomPlayersQuantity.ToString() );
        try
        {
            var options = new SessionOptions
            {
                Name = sessionName,
                MaxPlayers = playersLimit,
                IsPrivate = false
            }.WithRelayNetwork();

            Debug.Log( $"STARTING Session name: {sessionName}, players {playersLimit}" );
            await MultiplayerService.Instance.CreateOrJoinSessionAsync( sessionName, options );
            Debug.Log( $"SESSION STARTED" );
            _onSessionCreated?.Invoke();
        }
        catch
        {

        }
        //var lobbyName = PlayerPrefs.GetString( PlayerPrefsKeys.RoomName.ToString() );
        //var playersLimit = PlayerPrefs.GetInt( PlayerPrefsKeys.RoomPlayersQuantity.ToString() );
        //try
        //{
        //    var options = new CreateLobbyOptions
        //    {
        //        IsPrivate = false
        //    };

        //    Debug.Log( $"STARTING Lobby name: {lobbyName}, players {playersLimit}" );
        //    await LobbyService.Instance.CreateOrJoinLobbyAsync( lobbyName, lobbyName, playersLimit, options );
        //    Debug.Log( $"LOBBY READY" );

        //    OnLobbyStarted?.Invoke();
        //}
        //catch
        //{

        //}
    }
}
