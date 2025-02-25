using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CreateGameUI : MonoBehaviour
{
    [SerializeField] private InputField _lobbyName;
    [SerializeField] private Slider _playerCountSlider;
    [SerializeField] private Text _playerCountSliderText;
    [SerializeField] private Button _confirmButton;

    [SerializeField] private string _roomName;
    [SerializeField] private int _playerLimit;

    [SerializeField] private UnityEvent _onSessionCreated;

    private void Awake()
    {
        PlayerPrefs.SetString( PlayerPrefsKeys.RoomName.ToString(), "" );
        PlayerPrefs.Save();
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
        PlayerPrefs.Save();
    }

}