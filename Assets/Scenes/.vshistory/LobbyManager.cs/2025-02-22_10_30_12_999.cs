using Unity.Services.Multiplayer;
using UnityEngine;

public class LobbyManager : Singleton<LobbyManager>
{
    public async void StartSession()
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
        }
        catch
        {

        }
    }
}
