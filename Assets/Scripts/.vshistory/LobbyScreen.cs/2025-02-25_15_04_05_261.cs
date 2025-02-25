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

    [SerializeField] private List<PlayerLobbyCard> _cards;

    public event Action OnConfirm;

    public Dictionary<ulong, PlayerData> _players = new();

    private void Start()
    {
        _confirmGameButton.interactable = NetworkManager.Singleton.IsHost;

        _lobbyName.text = PlayerPrefs.GetString( PlayerPrefsKeys.RoomName.ToString() );
        CheckLobby( _lobbyName.text ).Forget();

        _confirmGameButton.onClick.AddListener( ConfirmButton );
        NetworkManager.Singleton.OnConnectionEvent += OnConnectionEvent;

        var name = PlayerPrefs.GetString( PlayerPrefsKeys.PlayerName.ToString() );
        var side = PlayerPrefs.GetString( PlayerPrefsKeys.PlayerSide.ToString() );

        UpdateHostPlayerListRpc( NetworkManager.Singleton.LocalClientId, name, side );
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

    [Rpc( SendTo.Server )]
    private void UpdateHostPlayerListRpc( ulong id, string playerName, string side )
    {
        var playerSide = ( Side ) Enum.Parse( typeof( Side ), side );
        var data = new PlayerData { PlayerName = playerName, PlayerSide = playerSide };

        if ( !_players.ContainsKey( id ) )
            _players.Add( id, data );
        else
            _players[ id ] = data;

        UpdatePlayersBox();
    }

    [Rpc( SendTo.Server )]
    private void UpdateHostPlayerListRpc( ulong id, Side side )
    {
        var data = _players[ id ];
        data.PlayerSide = side;

        _players[ id ] = data;

        UpdatePlayersBox();
    }



    private void UpdatePlayersBox()
    {
        var playerList = NetworkManager.Singleton.ConnectedClientsList;

        var sideBalance = 0;

        for ( int i = 0; i < 4; i++ )
        {
            var card = _cards[ i ];

            if ( i < playerList.Count )
            {
                var player = playerList[ i ];
                var data = _players[ player.ClientId ];
                card.Name = data.PlayerName;
                card.SideToggle = data.PlayerSide;

                sideBalance += data.PlayerSide == Side.Right ? 1 : -1;
            }
            else
            {
                var name = PlayerLobbyCard.RandomName;

                var side = Side.Left;

                if ( sideBalance < 0 )
                    side = Side.Right;

                card.name = name;
                card.SideToggle = side;
            }
        }
    }

    public void OnPlayerSideChange( ulong id, Side side )
    {
        UpdateHostPlayerListRpc( id, name, side );
    }
}

public class PlayerData
{
    public string PlayerName;
    public Side PlayerSide;
}
