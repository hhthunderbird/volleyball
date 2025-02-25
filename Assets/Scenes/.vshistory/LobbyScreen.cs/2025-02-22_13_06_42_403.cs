using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;
using Unity.Netcode;
using Unity.Services.Lobbies;
using UnityEngine;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;

public class LobbyScreen : MonoBehaviour
{
    [SerializeField] private Text _lobbyName;
    [SerializeField] private Text _playersCountText;
    [SerializeField] private Button _confirmGameButton;
    [SerializeField] private ClientSessionManager _clientSessionManager;

    public event Action OnConfirm;

    private void Awake()
    {
        _lobbyName.text = PlayerPrefs.GetString( PlayerPrefsKeys.RoomName.ToString());
        _confirmGameButton.onClick.AddListener( ConfirmButton );
        NetworkManager.Singleton.OnConnectionEvent += OnConnectionEvent;
    }

    private void OnEnable()
    {
        CheckLobby( _lobbyName.text ).Forget();
    }

    private void OnConnectionEvent( NetworkManager manager, ConnectionEventData data )
    {
        CheckLobby( _lobbyName.text ).Forget();
    }

    private void ConfirmButton()
    {
        SessionManager.Instance.Startgame();

        _clientSessionManager.StartGameRpc();
    }

    private async UniTaskVoid CheckLobby( string sessionName )
    {
        try
        {
            var lobby = await LobbyService.Instance.GetLobbyAsync( sessionName );

            Debug.Log( $"LOBBY COUNT {lobby.Players.Count}" );
            _playersCountText.text = lobby.Players.Count.ToString();
        }
        catch ( Exception e )
        {
            Debug.LogError( e.Message );
        }

    }
}
