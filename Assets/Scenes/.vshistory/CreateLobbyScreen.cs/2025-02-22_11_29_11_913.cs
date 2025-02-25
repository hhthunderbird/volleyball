using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public enum PlayerPrefsKeys
{
    RoomName,
    RoomPlayersQuantity
}
public class CreateLobbyScreen : MonoBehaviour
{
    [SerializeField] private InputField _lobbyName;
    [SerializeField] private Slider _playerCountSlider;
    [SerializeField] private Text _playerCountSliderText;
    [SerializeField] private Button _confirmButton;

    public UnityEvent LobbyStarted;

    private string _roomName;
    private int _playerLimit;

    private void Awake()
    {
        PlayerPrefs.SetString( PlayerPrefsKeys.RoomName.ToString(), "" );
        PlayerPrefs.Save();

        LobbyManager.Instance.OnLobbyStarted += OnLobbyStarted;
    }

    private void OnLobbyStarted()
    {
        
    }

    private void Start()
    {
        _lobbyName.onValueChanged.AddListener( RoomNameInputListener );
        _playerCountSlider.onValueChanged.AddListener( PlayerCountSliderListener );
        RoomNameInputListener( "" );
        PlayerCountSliderListener( _playerCountSlider.value );
    }

    private void RoomNameInputListener( string value )
    {
        var roomName = value;
        if ( string.IsNullOrEmpty( roomName ) )
        {
            roomName = Random.Range( 1000, 9999 ).ToString();
        }

        _lobbyName.text = _roomName = roomName;
    }

    private void PlayerCountSliderListener( float value )
    {
        var quantity = ( int ) value;
        _playerLimit = quantity;
        _playerCountSliderText.text = _playerLimit.ToString();
    }

    public void ConfirmButton()
    {
        _confirmButton.interactable = false;
        PlayerPrefs.SetString( PlayerPrefsKeys.RoomName.ToString(), _roomName );
        PlayerPrefs.SetInt( PlayerPrefsKeys.RoomPlayersQuantity.ToString(), _playerLimit );
        PlayerPrefs.Save();

        LobbyManager.Instance.StartLobby();
    }
}
