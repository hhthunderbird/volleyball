using Cysharp.Threading.Tasks;
using System.Collections;
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

        if ( _searchRoomRoutine != null )
            StopCoroutine( _searchRoomRoutine );
        _searchRoomRoutine = StartCoroutine( SearchRoomRoutine() );
        _confirmButton.interactable = false;
    }

    private IEnumerator SearchRoomRoutine()
    {

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
        _confirmButton.interactable = !string.IsNullOrEmpty( lobby );
    }
}
