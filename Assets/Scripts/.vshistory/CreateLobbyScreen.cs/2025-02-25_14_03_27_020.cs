using System.Collections.Generic;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public enum PlayerPrefsKeys
{
    RoomName,
    RoomPlayersQuantity,
    PlayerName,
    PlayerSide
}
public class CreateLobbyScreen : MonoBehaviour
{
    [SerializeField] private InputField _lobbyName;
    [SerializeField] private Button _confirmButton;

    [SerializeField] private Slider _playerCountSlider;
    [SerializeField] private Text _playerCountSliderText;

    [SerializeField] private Text _playerName;
    [SerializeField] private InputField _playerNameInput;

    public UnityEvent LobbyStarted;

    private int _playerLimit;

    private void Awake()
    {
        PlayerPrefs.SetString( PlayerPrefsKeys.RoomName.ToString(), "" );
        PlayerPrefs.Save();
    }

    private void Start()
    {
        _lobbyName.onValueChanged.AddListener( RoomNameInputListener );
        _playerCountSlider.onValueChanged.AddListener( PlayerCountSliderListener );

        _confirmButton.onClick.AddListener( ConfirmButton );
        _playerNameInput.onValueChanged.AddListener( OnPlayerNameInput );

        RoomNameInputListener( "" );
        PlayerCountSliderListener( _playerCountSlider.value );
        OnPlayerNameInput( PlayerLobbyCard.RandomName );
    }

    private void OnPlayerNameInput( string value )
    {
        PlayerPrefs.SetString( PlayerPrefsKeys.PlayerName.ToString(), value );
    }

    private void RoomNameInputListener( string value )
    {
        var roomName = value;
        
        _lobbyName.text = roomName;
    }

    private void PlayerCountSliderListener( float value )
    {
        var quantity = ( int ) value;
        _playerLimit = quantity;
        _playerCountSliderText.text = _playerLimit.ToString();
    }

    private Dictionary<string, PlayerProperty> GetPlayerProperties()
    {
        var playerName = _playerNameInput.text;
        var playerSide = Side.Left;

        var nameProp = new PlayerProperty( playerName );
        var sideProp = new PlayerProperty( playerSide.ToString() );

        return new Dictionary<string, PlayerProperty> { { "name", nameProp }, { "side", sideProp } };
    }

    public async void ConfirmButton()
    {
        if ( string.IsNullOrEmpty( _lobbyName.text ) )
            RoomNameInputListener(Random.Range( 1000, 9999 ).ToString());
        
        if ( string.IsNullOrEmpty( _playerNameInput.text ) )
            OnPlayerNameInput( PlayerLobbyCard.RandomName );


        _confirmButton.interactable = false;
        PlayerPrefs.SetString( PlayerPrefsKeys.RoomName.ToString(), _lobbyName.text );
        PlayerPrefs.SetString( PlayerPrefsKeys.PlayerName.ToString(), _playerName.text);
        PlayerPrefs.SetInt( PlayerPrefsKeys.RoomPlayersQuantity.ToString(), _playerLimit );
        PlayerPrefs.Save();

        Debug.Log( "starting lobby" );
        try
        {
            await SessionManager.Instance.StartSession( _lobbyName.text );
        }
        catch{}

        Debug.Log( "lobby started" );
        LobbyStarted?.Invoke();
    }
}
