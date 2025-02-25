using Cysharp.Threading.Tasks;
using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : Singleton<LobbyManager>
{
    public event Action OnLobbyStarted;


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

    public async UniTask StartLobby()
    {
        var lobbyName = PlayerPrefs.GetString( PlayerPrefsKeys.RoomName.ToString() );
        var playersLimit = PlayerPrefs.GetInt( PlayerPrefsKeys.RoomPlayersQuantity.ToString() );

        await UnityServices.Instance.InitializeAsync();

        try
        {
            var options = new CreateLobbyOptions
            {
                IsPrivate = false
            };

            Debug.Log( $"STARTING Lobby name: {lobbyName}, players {playersLimit}" );
            await LobbyService.Instance.CreateOrJoinLobbyAsync( lobbyName, lobbyName, playersLimit, options );
            Debug.Log( $"LOBBY READY" );

            OnLobbyStarted?.Invoke();
        }
        catch
        {

        }
    }
}
