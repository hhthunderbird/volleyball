using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ImageSelectionUI : NetworkBehaviour
{
    [SerializeField] private Text _lobbyName;
    [SerializeField] private Button _confirmGameButton;
    [SerializeField] private Text _playersCountText;
    private int _playersCount;

    private void Awake()
    {
        _confirmGameButton.onClick.AddListener( ConfirmButton );
    }

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.OnConnectionEvent += OnConnectionEvent;

    }

    private void OnConnectionEvent( NetworkManager manager, ConnectionEventData data )
    {
        switch ( data.EventType )
        {
            case ConnectionEvent.ClientConnected:
                Debug.Log( $"açsdflaçsdkfljasçdfkl ::: {NetworkManager.Singleton.ConnectedClientsList.Count}" );
                _playersCount = NetworkManager.Singleton.ConnectedClientsList.Count;
                _playersCountText.text = _playersCount.ToString();
                break;
            case ConnectionEvent.PeerConnected:
                break;
            case ConnectionEvent.ClientDisconnected:
                break;
            case ConnectionEvent.PeerDisconnected:
                break;
            default:
                break;
        }
    }

    private void ConfirmButton()
    {
        SessionManager.Instance.Startgame();
    }
}