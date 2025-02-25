using System.IO;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ImageSelectionUI : NetworkBehaviour
{
    [SerializeField] private Text _lobbyName;
    [SerializeField] private Button _startGameButton;

    private void Awake()
    {
        _startGameButton.gameObject.SetActive( false );
    }

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.OnSessionOwnerPromoted += OnSessionOwnerPromoted;
    }

    private void OnSessionOwnerPromoted( ulong sessionOwnerPromoted )
    {
        if ( sessionOwnerPromoted == NetworkManager.Singleton.LocalClient.ClientId )
        {
            
        }
    }

    private void Start()
    {
        if ( IsOwner )
        {
            _startGameButton.gameObject.SetActive( true );
        }

        _startGameButton.onClick.AddListener( StartGameButton );
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