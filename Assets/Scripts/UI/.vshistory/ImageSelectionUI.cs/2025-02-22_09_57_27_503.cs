using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ImageSelectionUI : NetworkBehaviour
{
    [SerializeField] private Text _lobbyName;
    [SerializeField] private Button _confirmGameButton;
    [SerializeField] private Text _playersCountText;
    [SerializeField] private ClientSessionManager _clientSessionManager;

    public event Action OnConfirm;

    private void Awake()
    {
        _confirmGameButton.onClick.AddListener( ConfirmButton );
    }

    private void ConfirmButton()
    {
        SessionManager.Instance.Startgame();

        _clientSessionManager.StartGameRpc();
    }

    [Rpc( SendTo.Everyone )]
    public void StartGameRpc()
    {
        _playersCountText.text = NetworkManager.Singleton.ConnectedClientsList.Count.ToString();
    }

}