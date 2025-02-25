using System.IO;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ImageSelectionUI : NetworkBehaviour
{
    [SerializeField] private Texture2D _selectedImage;
    [SerializeField] private Image _selectedUIImage;

    [SerializeField] private Text _lobbyName;
    [SerializeField] private Button _lessPiecesButton;
    [SerializeField] private Button _morePiecesButton;
    [SerializeField] private Text _totalPieces;
    [SerializeField] private Button _startGameButton;


    [SerializeField] private ImageTransfer _imageTransfer;

    private int _minLength = 3;
    private int _maxLength = 100;

    private NetworkVariable<int> _width = new();

    private NativeFilePicker.Permission _permission;

    private byte[] _imageData;

    private void Awake()
    {
        _startGameButton.gameObject.SetActive( false );
        _imageTransfer.OnReceivedImage += OnReceivedImage;
    }

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.OnSessionOwnerPromoted += OnSessionOwnerPromoted;
        SessionManager.Instance.OnClientConnection += OnClientConnection;
        _totalPieces.text = TotalPieces();
    }

    private void OnClientConnection( ulong clientId )
    {
        Debug.Log( $"CLIENT '{clientId}' CONNECTED {_imageData != null} && {NetworkManager.Singleton.ConnectedClients.Count}" );
        //TRY TO SEND JUST TO ONE CLIENT THAT IS ENTERING
        if ( _imageData != null )
        {
            _imageTransfer.SendImageToPeers( _imageData );
        }
    }

    private void OnReceivedImage( Texture2D texture )
    {
        _selectedImage = texture;
        ApplySelectedImage();
    }

    private void OnSessionOwnerPromoted( ulong sessionOwnerPromoted )
    {
        if ( sessionOwnerPromoted == NetworkManager.Singleton.LocalClient.ClientId )
        {
            _startGameButton.gameObject.SetActive( true );
        }
    }

    private void Start()
    {
        if ( IsOwner )
        {
            _startGameButton.gameObject.SetActive( true );
        }

        _lessPiecesButton.onClick.AddListener( LessPiecesButtonListener );
        _morePiecesButton.onClick.AddListener( MorePiecesButtonListener );
        _startGameButton.onClick.AddListener( StartGameButton );

        _width.Value = 3;
        _totalPieces.text = TotalPieces();

        RequestPermissionAsynchronously( true );
    }

    private void LessPiecesButtonListener()
    {
        _morePiecesButton.interactable = true;

        _width.Value--;
        if ( _width.Value == _minLength )
            _lessPiecesButton.interactable = false;

        _totalPieces.text = TotalPieces();
    }

    private void MorePiecesButtonListener()
    {
        _lessPiecesButton.interactable = true;

        _width.Value++;
        if ( _width.Value == _maxLength )
            _morePiecesButton.interactable = false;

        _totalPieces.text = TotalPieces();
    }

    private string TotalPieces()
    {
        if ( _selectedImage == null ) return "0";

        var horizontalSize = _selectedImage.width / ( float ) _width.Value;

        var height = ( int ) ( _selectedImage.height / horizontalSize );

        return ( _width.Value * height ).ToString();
    }

    public void ConfirmButton()
    {
        //PlayerPrefs.SetInt( PlayerPrefsKeys.Width.ToString(), _width.Value );
        //PlayerPrefs.SetInt( PlayerPrefsKeys.TextureId.ToString(), _textureId );
        //PlayerPrefs.Save();
    }

    public void OpenFileBrowser()
    {
        // Don't attempt to import/export files if the file picker is already open
        if ( NativeFilePicker.IsFilePickerBusy() )
            return;

        if ( _permission != NativeFilePicker.Permission.Granted )
        {
            RequestPermissionAsynchronously( true );
            return;
        }

#if UNITY_ANDROID
        // Use MIMEs on Android
        string[] fileTypes = new string[] { "image/*"/*, "video/*"*/ };
#else
			// Use UTIs on iOS
			string[] fileTypes = new string[] { "public.image"/*, "public.movie"*/ };
#endif

        // Pick image(s) and/or video(s)
        NativeFilePicker.Permission permission = NativeFilePicker.PickFile( ( path ) =>
        {
            if ( path == null )
                Debug.Log( "Operation cancelled" );
            else
            {
                Debug.Log( "Picked file: " + path );
                _imageData = File.ReadAllBytes( path );

                if ( IsOwner )
                {
                    _selectedImage = _imageTransfer.StreamToTexture( _imageData );

                    ApplySelectedImage();
                }

                Debug.Log( $"TEST {_imageData != null} && {NetworkManager.Singleton.ConnectedClientsList.Count}" );

                if ( _imageData != null )
                    _imageTransfer.SendImageToPeers( _imageData );
            }
        }, fileTypes );

        Debug.Log( "Permission result: " + permission );
    }

    private void ApplySelectedImage()
    {
        Debug.Log( $"image received? {_selectedImage != null}" );
        if ( _selectedImage == null ) return;

        Sprite sprite = Sprite.Create( _selectedImage, new Rect( 0, 0, _selectedImage.width, _selectedImage.height ), Vector2.zero );

        _selectedUIImage.sprite = sprite;
        TextureManager.Instance.Texture = _selectedImage;
    }


    // Example code doesn't use this function but it is here for reference. It's recommended to ask for permissions manually using the
    // RequestPermissionAsync methods prior to calling NativeFilePicker functions
    private async void RequestPermissionAsynchronously( bool readPermissionOnly = false )
    {
        _permission = await NativeFilePicker.RequestPermissionAsync( readPermissionOnly );
        Debug.Log( "Permission result: " + _permission );
    }

    private void StartGameButton()
    {
        //SessionManager.Instance.Startgame();
    }

    [Rpc(SendTo.Server)]
    public void SendReadyToServerRpc()
    {
        CheckReadyStateRpc();
    }

    public void CheckReadyStateRpc()
    {

    }



    [Rpc(SendTo.Everyone)]
    public void SendConfirmToEveryoneRpc()
    {
        ConfirmMeRpc();
    }

    [Rpc(SendTo.Me)]
    public void ConfirmMeRpc()
    {

    }
}