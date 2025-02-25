using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Multiplayer;
using UnityEngine;

public class LobbyManager : Singleton<LobbyManager>
{
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
