using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScreen : NetworkBehaviour
{
    [SerializeField] private Text _lobbyName;
    [SerializeField] private Text _playersCountText;
    [SerializeField] private Button _confirmGameButton;

    [SerializeField] private Transform _playersCardsContainer;

    public event Action OnConfirm;

    public Dictionary<ulong, PlayerData> _players = new();

    private string _playerName;
    private Side _playerSide;

    private void Start()
    {
        _confirmGameButton.interactable = NetworkManager.Singleton.IsHost;

        _lobbyName.text = PlayerPrefs.GetString( PlayerPrefsKeys.RoomName.ToString() );
        CheckLobby( _lobbyName.text ).Forget();

        _confirmGameButton.onClick.AddListener( ConfirmButton );
        NetworkManager.Singleton.OnConnectionEvent += OnConnectionEvent;
    }

    private void OnConnectionEvent( NetworkManager manager, ConnectionEventData data )
    {
        CheckLobby( _lobbyName.text ).Forget();
    }

    private void ConfirmButton()
    {
        SessionManager.Instance.StartGameRpc();
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

    [Rpc(SendTo.Server)]
    private void UpdateHostPlayerListRpc(ulong id, string playerName, string side )
    {
        if ( !_players.ContainsKey( id ) )
        {
            Side playerSide = ( Side ) Enum.Parse(typeof(Side), side );

            var data = new PlayerData { PlayerName = playerName, Si }
            _players.Add(id,  )
        }
    }
}

public class PlayerData
{
    public string PlayerName;
    public Side PlayerSide;
}
