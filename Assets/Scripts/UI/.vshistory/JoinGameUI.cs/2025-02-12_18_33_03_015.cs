using Cysharp.Threading.Tasks;
using Unity.Tutorials.Core.Editor;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameUI : MonoBehaviour
{

    [SerializeField] private InputField _lobbyName;
    [SerializeField] private Button _confirmButton;
    private Coroutine _searchRoomRoutine;

    private void OnEnable()
    {
        SetLobbyName( _lobbyName.text );
        _confirmButton.interactable = false;
    }

    private void Start()
    {
        _lobbyName.onValueChanged.AddListener( SetLobbyName );
        _lobbyName.text = ClientInfo.LobbyName;
    }

    private async UniTask CheckRoomExistence()
    {
        if ( !_lobbyName.text.IsNullOrEmpty() )
        {

        }
    }

    private void SetLobbyName( string lobby )
    {
        ClientInfo.LobbyName = lobby;
        _confirmButton.interactable = !string.IsNullOrEmpty(lobby);
    }
}
