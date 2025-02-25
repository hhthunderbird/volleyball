using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CreateGameUI : MonoBehaviour
{
    [SerializeField] private InputField lobbyName;
    //[SerializeField] private Dropdown track;
    //[SerializeField] private Dropdown gameMode;
    [SerializeField] private Slider playerCountSlider;
    [SerializeField] private Slider difficultyCountSlider;
    [SerializeField] private Image trackImage;
    [SerializeField] private Text playerCountSliderText;
    [SerializeField] private Image playerCountIcon;
    [SerializeField]
    private Button confirmButton
        [ SerializeField ] private Transform trackModelPlace;
    //private GameObject loadedModel;

    //resources
    public Sprite padlockSprite, publicLobbyIcon;

    //private void Start()
    //{
    //    playerCountSlider.SetValueWithoutNotify( 8 );
    //    SetPlayerCount();

    //    SetTrackDropdown();

    //    gameMode.ClearOptions();
    //    gameMode.AddOptions( ResourceManager.Instance.gameTypes.Select( x => x.modeName ).ToList() );
    //    gameMode.onValueChanged.AddListener( SetGameType );
    //    SetGameType( 0 );

    //    playerCountSlider.wholeNumbers = true;
    //    playerCountSlider.minValue = 1;
    //    playerCountSlider.maxValue = 8;
    //    playerCountSlider.value = 2;
    //    playerCountSlider.onValueChanged.AddListener( x => ServerInfo.MaxUsers = (int)x );

    //    lobbyName.onValueChanged.AddListener( x =>
    //    {
    //        ServerInfo.LobbyName = x;
    //        confirmButton.interactable = !string.IsNullOrEmpty( x );
    //    } );
    //    lobbyName.text = ServerInfo.LobbyName = "Session" + Random.Range( 0, 1000 );

    //    ServerInfo.TrackId = track.options[0].text;
    //    ServerInfo.GameMode = gameMode.value;
    //    ServerInfo.MaxUsers = (int)playerCountSlider.value;
    //}

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
    //    ServerInfo.TrackId = track.options[ trackId].text;
    //    Debug.LogError($"ServerInfo.TrackId: {ServerInfo.TrackId}");

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
    //    if(_lobbyIsValid)
    //    {
    //        UIScreen.Focus( screen );
    //    }
    //}

    //public void TryCreateLobby( GameLauncher launcher )
    //{
    //    if(_lobbyIsValid)
    //    {
    //        //Destroy( loadedModel );
    //        launcher.JoinOrCreateLobby();
    //        _lobbyIsValid = false;
    //    }
    //}
}