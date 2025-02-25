using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSelectionUI : MonoBehaviour
{
    [SerializeField] private List<Texture2D> _images;

    [SerializeField] private InputField _lobbyName;
    [SerializeField] private Button _lessPiecesButton;
    [SerializeField] private Button _morePiecesButton;
    [SerializeField] private Text _totalPieces;

    private string _roomName;
    private int _playerLimit;

    private int _minLength;
    private int _maxLength;


    private int _length;

    private void Awake()
    {
        PlayerPrefs.SetString( PlayerPrefsKeys.RoomName.ToString(), "" );
        PlayerPrefs.Save();
    }

    private void Start()
    {
        _lessPiecesButton.onClick.AddListener( LessPiecesButtonListener );
        _morePiecesButton.onClick.AddListener( MorePiecesButtonListener );
    }

    private void LessPiecesButtonListener()
    {
        _length--;
        if ( _length == _minLength )
            _lessPiecesButton.interactable = false;

        _totalPieces.text = TotalPieces().ToString();
    }

    private void MorePiecesButtonListener()
    {

    }

    private int TotalPieces()
    {
        _length = 15;
        var horizontalSize = _mainImage.width / ( float ) _length;

        var height = ( int ) ( _mainImage.height / horizontalSize );
        var finalVerticalSize = _mainImage.height / ( float ) height;
        var verticalScale = finalVerticalSize / horizontalSize;
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