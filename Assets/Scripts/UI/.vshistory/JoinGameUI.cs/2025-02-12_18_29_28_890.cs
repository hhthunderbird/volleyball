using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameUI : MonoBehaviour
{

    [SerializeField] private InputField lobbyName;
    [SerializeField] private Button _confirmButton;

    private void OnEnable()
    {
        SetLobbyName( lobbyName.text );
        _confirmButton.interactable = false;
    }

    private void Start()
    {
        lobbyName.onValueChanged.AddListener( SetLobbyName );
        lobbyName.text = ClientInfo.LobbyName;
    }

    private async void CheckRoomExistence()
    {
        if(!lobbyName.text.IsNullOrEmpty())
    }

    private void SetLobbyName( string lobby )
    {
        ClientInfo.LobbyName = lobby;
        _confirmButton.interactable = !string.IsNullOrEmpty(lobby);
    }
}
