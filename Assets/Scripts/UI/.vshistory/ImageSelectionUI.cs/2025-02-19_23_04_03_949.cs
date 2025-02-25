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
        NetworkManager.Singleton.OnSessionOwnerPromoted += OnSessionOwnerPromoted;
    }

    private void OnSessionOwnerPromoted( ulong sessionOwnerPromoted )
    {
        if ( sessionOwnerPromoted == NetworkManager.Singleton.LocalClient.ClientId )
        {

        }
    }

    private void ConfirmButton()
    {
        SessionManager.Instance.Startgame();
    }
}