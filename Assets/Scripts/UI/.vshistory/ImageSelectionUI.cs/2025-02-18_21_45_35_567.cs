using System;
using System.Collections.Generic;
using System.IO;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ImageSelectionUI : NetworkBehaviour
{
    [SerializeField] private Text _lobbyName;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _readyGameButton;

    

    private void Awake()
    {
        _startGameButton.gameObject.SetActive( false );
        _readyGameButton.onClick.AddListener( OnReadyState );
    }

    private void OnReadyState()
    {
        var value = SessionManager.Instance.CheckReadyState();
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

    [Rpc( SendTo.Server )]
    private void SendReadyToServerRpc(ulong clientId)
    {
        CheckReadyStateRpc(clientId);
    }

    public void CheckReadyStateRpc( ulong clientId )
    {

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