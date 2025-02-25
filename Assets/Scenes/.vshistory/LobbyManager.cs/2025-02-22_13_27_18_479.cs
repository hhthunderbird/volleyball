using Cysharp.Threading.Tasks;
using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : Singleton<LobbyManager>
{
    private async void Start()
    {
        try
        {
            await UnityServices.Instance.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            Debug.Log( $"User authenticated  {AuthenticationService.Instance.PlayerId}" );
        }
        catch
        {

        }
    }

    public async StartLobby()
    {
        var lobbyName = PlayerPrefs.GetString( PlayerPrefsKeys.RoomName.ToString() );
        var playersLimit = PlayerPrefs.GetInt( PlayerPrefsKeys.RoomPlayersQuantity.ToString() );

        var options = new CreateLobbyOptions
        {
            IsPrivate = false
        };

        return LobbyService.Instance.CreateOrJoinLobbyAsync( lobbyName, lobbyName, playersLimit, options );
    }
}
