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

    public event Action OnConfirm;

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
        //if ( !IsOwner ) return;


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


    [Rpc( SendTo.Authority )]
    private void StartGameRpc()
    {
        StartgameActionRpc();
    }

    [Rpc( SendTo.NotServer )]
    private void StartgameActionRpc()
    {
        Debug.Log( $"client {NetworkManager.Singleton.LocalClientId} rpc response" );   
    }




    //private void Update()
    //{
    //    if ( Input.GetKeyDown( KeyCode.F9 ) )
    //        _playersCount++;
    //    if ( Input.GetKeyDown( KeyCode.F10 ) )
    //        OnScore( PlayerScore.Right );
    //}

    //[Rpc( SendTo.Authority )]
    //private void ScoreboardCallRpc()
    //{
    //    ScoreboardUpdateRpc();
    //}

    //[Rpc( SendTo.Me )]
    //private void ScoreboardUpdateRpc()
    //{
    //    _rightPlayerScore.text = _rightScore.Value.ToString();
    //    _leftPlayerScore.text = _leftScore.Value.ToString();
    //}
}