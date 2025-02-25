using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ImageSelectionUI : NetworkBehaviour
{
    [SerializeField] private Text _lobbyName;
    [SerializeField] private Button _confirmGameButton;
    [SerializeField] private Text _playersCountText;
    private NetworkVariable<int> _playersCount = new();

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
        throw new NotImplementedException();
    }

    private void ConfirmButton()
    {
        SessionManager.Instance.Startgame();
    }
}