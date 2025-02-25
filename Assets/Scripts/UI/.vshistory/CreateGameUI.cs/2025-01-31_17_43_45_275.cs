using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CreateGameUI : MonoBehaviour
{
    [SerializeField] private InputField _lobbyName;
    [SerializeField] private Slider _playerCountSlider;
    [SerializeField] private Text _playerCountSliderText;
    [SerializeField] private Slider _difficultyCountSlider;
    [SerializeField] private Text _piecesCountSliderText;
    [SerializeField] private Button _confirmButton;
    //[SerializeField] private Image trackImage;

    private void Start()
    {
        _lobbyName.onValueChanged.AddListener( RoomNameInputListener );
    }

    private void RoomNameInputListener(string value )
    {
        if ( string.IsNullOrEmpty( value ) )
        {
            _lobbyName.text = ((int)Random.value * 1000).ToString();
        }
        PlayerPrefs.SetString( PlayerPrefsKeys.RoomName.ToString(), value );
    }

    private void Starta()
    {
        //playerCountSlider.SetValueWithoutNotify( 8 );
        //SetPlayerCount();

        //SetTrackDropdown();

        //gameMode.ClearOptions();
        //gameMode.AddOptions( ResourceManager.Instance.gameTypes.Select( x => x.modeName ).ToList() );
        //gameMode.onValueChanged.AddListener( SetGameType );
        //SetGameType( 0 );

        //playerCountSlider.wholeNumbers = true;
        //playerCountSlider.minValue = 1;
        //playerCountSlider.maxValue = 8;
        //playerCountSlider.value = 2;
        //playerCountSlider.onValueChanged.AddListener( x => ServerInfo.MaxUsers = ( int ) x );

        //lobbyName.onValueChanged.AddListener( x =>
        //{
        //    ServerInfo.LobbyName = x;
        //    confirmButton.interactable = !string.IsNullOrEmpty( x );
        //} );
        //lobbyName.text = ServerInfo.LobbyName = "Session" + Random.Range( 0, 1000 );

        //ServerInfo.TrackId = track.options[ 0 ].text;
        //ServerInfo.GameMode = gameMode.value;
        //ServerInfo.MaxUsers = ( int ) playerCountSlider.value;
    }

    //private void SetTrackDropdown()
    //{
    //    track.ClearOptions();
    //    track.AddOptions( ResourceManager.Instance.tracks.Select( x => x.trackName ).ToList() );
    //    track.onValueChanged.AddListener( SetTrack );
    //    SetTrack( 0 );
    //}

    //public void SetGameType( int gameType )
    //{
    //    ServerInfo.GameMode = gameType;
    //}

    //public void SetTrack( int trackId )
    //{
    //    //Destroy( loadedModel );
    //    ServerInfo.TrackId = track.options[ trackId ].text;
    //    Debug.LogError( $"ServerInfo.TrackId: {ServerInfo.TrackId}" );

    //    //TODO: Create event system
    //    ResourceManager.Instance.TrackId = ServerInfo.TrackId;
    //    trackImage.sprite = ResourceManager.Instance.tracks[ trackId ].trackIcon;
    //    //loadedModel = Instantiate( ResourceManager.Instance.trackModels[trackId], Vector3.zero, Quaternion.identity, trackModelPlace);
    //}

    //public void SetPlayerCount()
    //{
    //    playerCountSlider.value = ServerInfo.MaxUsers;
    //    playerCountSliderText.text = $"{ServerInfo.MaxUsers}";
    //    playerCountIcon.sprite = ServerInfo.MaxUsers > 1 ? publicLobbyIcon : padlockSprite;
    //}

    //// UI Hooks

    //private bool _lobbyIsValid;

    //public void ValidateLobby()
    //{
    //    _lobbyIsValid = string.IsNullOrEmpty( ServerInfo.LobbyName ) == false;
    //}

    //public void TryFocusScreen( UIScreen screen )
    //{
    //    if ( _lobbyIsValid )
    //    {
    //        UIScreen.Focus( screen );
    //    }
    //}

    //public void TryCreateLobby( GameLauncher launcher )
    //{
    //    if ( _lobbyIsValid )
    //    {
    //        //Destroy( loadedModel );
    //        launcher.JoinOrCreateLobby();
    //        _lobbyIsValid = false;
    //    }
    //}
}