using System;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : Singleton<LobbyManager>
{
    [SerializeField] private Button _confirmButton;
    public event Action OnLobbyStarted;


    private void Start()
    {
        _confirmButton.onClick.AddListener( StartLobby );
    }

    public async void StartLobby()
    {
        _confirmButton.interactable = false;
        var lobbyName = PlayerPrefs.GetString( PlayerPrefsKeys.RoomName.ToString() );
        var playersLimit = PlayerPrefs.GetInt( PlayerPrefsKeys.RoomPlayersQuantity.ToString() );
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
