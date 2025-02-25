using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class ImageSelectionUI : NetworkBehaviour
{
    [SerializeField] private Text _lobbyName;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _readyGameButton;
    private bool _readyState;


    private void Awake()
    {
        _startGameButton.gameObject.SetActive( false );
        _readyGameButton.onClick.AddListener( OnReadyState );
    }

    private void OnReadyState()
    {
        var id = NetworkManager.Singleton.LocalClientId;

        SendReadyToServerRpc( id );

        //SessionManager.Instance.SetClientState( id, _readyState );

        //var value = SessionManager.Instance.CheckReadyState();
        //Debug.Log( $"açsdkflaçsdflkjasçkdfjl {value}" );
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

    private void Start()
    {
        if ( IsOwner )
        {
            _startGameButton.gameObject.SetActive( true );
        }

        _startGameButton.onClick.AddListener( StartGameButton );
    }

    public void ConfirmButton()
    {

    }

    private void StartGameButton()
    {
        SessionManager.Instance.Startgame();
    }

    [Rpc( SendTo.Authority)]
    private void SendReadyToServerRpc( ulong clientId )
    {
        CheckReadyStateRpc( clientId );
    }

    [Rpc(SendTo.Me)]
    private void CheckReadyStateRpc( ulong clientId )
    {
        Debug.Log( $"client id {clientId}" );
        _readyState = !_readyState;
    }



    //[Rpc(SendTo.Everyone)]
    //public void SendConfirmToEveryoneRpc()
    //{
    //    ConfirmMeRpc();
    //}

    //[Rpc(SendTo.Me)]
    //public void ConfirmMeRpc()
    //{

    //}
}