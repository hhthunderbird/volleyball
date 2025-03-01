using Cysharp.Threading.Tasks;
using System;
using Unity.Netcode;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScreen : NetworkBehaviour
{
    [SerializeField] private Text _lobbyName;
    [SerializeField] private Text _playersCountText;
    [SerializeField] private Button _confirmGameButton;
    [SerializeField] private ClientSessionManager _clientSessionManager;

    public event Action OnConfirm;

    private void Start()
    {
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
        _clientSessionManager.StartGameRpc();
        _clientSessionManager.TesteClientRpc();
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
