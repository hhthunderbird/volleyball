using System;
using Unity.Netcode;
using Unity.Services.Multiplayer;
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
        SessionManager.Instance.OnClientConnection += OnClientConnection;
    }

    public override void OnNetworkSpawn()
    {
    }

    private void OnClientConnection( ulong id )
    {
        //if ( !IsOwner || !HasAuthority ) return;

        Debug.Log( $"{id} ::: {NetworkManager.Singleton.LocalClientId} ::: {IsOwner} && {HasAuthority}" );

        _playersCount = SessionManager.Instance.CurrentSession.Players.Count;
        _playersCountText.text = _playersCount.ToString();
    }

    private void ConfirmButton()
    {
        SessionManager.Instance.Startgame();
        StartGameRpc();
    }


    [Rpc( SendTo.Server )]
    private void StartGameRpc()
    {
        StartgameActionRpc();
    }

    [Rpc( SendTo.Authority )]
    private void StartgameActionRpc()
    {
        Debug.Log( $"client {NetworkManager.Singleton.LocalClientId} rpc response" );
        //Debug.Log( $"client {NetworkManager.Singleton.LocalClientId} rpc response" );   
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