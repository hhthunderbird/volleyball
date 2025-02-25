using System;
using Unity.Services.Lobbies;
using UnityEngine;

public class LobbyManager : Singleton<LobbyManager>
{
    public event Action OnLobbyStarted;

    public async void StartLobby()
    {
        var lobbyName = PlayerPrefs.GetString( PlayerPrefsKeys.RoomName.ToString() );
        var playersLimit = PlayerPrefs.GetInt( PlayerPrefsKeys.RoomPlayersQuantity.ToString() );
        try
        {
            var options = new CreateLobbyOptions
            {
                IsPrivate = false
            };

            Debug.Log( $"STARTING Session name: {lobbyName}, players {playersLimit}" );
            await LobbyService.Instance.CreateOrJoinLobbyAsync( lobbyName, lobbyName, playersLimit, options );
            Debug.Log( $"SESSION STARTED" );
        }
        catch
        {

        }
    }
}
