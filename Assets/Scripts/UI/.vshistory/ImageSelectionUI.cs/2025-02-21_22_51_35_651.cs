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
    }

    private void ConfirmButton()
    {
        SessionManager.Instance.Startgame();
        StartGameRpc();
    }


    [Rpc( SendTo.ClientsAndHost )]
    private void StartGameRpc()
    {
        StartgameActionRpc();
    }

    [Rpc( SendTo.NotServer )]
    private void StartgameActionRpc()
    {
        Debug.Log( $"client rpc response" );
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