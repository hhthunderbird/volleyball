using System;
using System.Collections.Generic;
using System.IO;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ImageSelectionUI : NetworkBehaviour
{
    [SerializeField] private List<Texture2D> _images;
    [SerializeField] private Texture2D _selectedImage;
    private Image _selectedUIImage;

    [SerializeField] private Text _lobbyName;
    [SerializeField] private Button _lessPiecesButton;
    [SerializeField] private Button _morePiecesButton;
    [SerializeField] private Text _totalPieces;
    [SerializeField] private GameObject _startButton;
    
    
    [SerializeField] private ImageTransfer _imageTransfer;

    private int _minLength = 3;
    private int _maxLength = 100;

    private int _width;
    private int _textureId;


    private NativeFilePicker.Permission _permission;

    private const int ChunkSize = 1024 * 32; // 32 KB per chunk
    private byte[] _imageData;// = File.ReadAllBytes( imagePath );

    private void Awake()
    {
        _startButton.SetActive( false );

        PlayerPrefs.SetString( PlayerPrefsKeys.RoomName.ToString(), "" );
        PlayerPrefs.Save();

        NetworkManager.Singleton.OnSessionOwnerPromoted += OnSessionOwnerPromoted;

        _imageTransfer.OnReceivedImage += OnReceivedImage;
    }

    private void OnReceivedImage( Texture2D image )
    {
        _selectedImage = image;
    }

    private void OnSessionOwnerPromoted( ulong sessionOwnerPromoted )
    {
        if(sessionOwnerPromoted == NetworkManager.Singleton.LocalClient.ClientId )
        {
            _startButton.SetActive( true );
        }
    }

    private void Start()
    {
        if ( IsOwner )
        {
            _startButton.SetActive( true );
        }

        _lessPiecesButton.onClick.AddListener( LessPiecesButtonListener );
        _morePiecesButton.onClick.AddListener( MorePiecesButtonListener );
        
        _width = 3;
        _totalPieces.text = TotalPieces();

        RequestPermissionAsynchronously( true );
    }

    private void LessPiecesButtonListener()
    {
        _morePiecesButton.interactable = true;

        _width--;
        if ( _width == _minLength )
            _lessPiecesButton.interactable = false;

        _totalPieces.text = TotalPieces();
    }

    private void MorePiecesButtonListener()
    {
        _lessPiecesButton.interactable = true;

        _width++;
        if ( _width == _maxLength)
            _morePiecesButton.interactable = false;

        _totalPieces.text = TotalPieces();
    }

    private string TotalPieces()
    {
        var horizontalSize = _selectedImage.width / ( float ) _width;

        var height = ( int ) ( _selectedImage.height / horizontalSize );

        return ( _width * height ).ToString();
    }

    public void ConfirmButton()
    {
        PlayerPrefs.SetInt( PlayerPrefsKeys.Width.ToString(), _width );
        PlayerPrefs.SetInt( PlayerPrefsKeys.TextureId.ToString(), _textureId );
        PlayerPrefs.Save();
    }

    public void OpenFileBrowser()
    {
        // Don't attempt to import/export files if the file picker is already open
        if ( NativeFilePicker.IsFilePickerBusy() )
            return;

        if( _permission != NativeFilePicker.Permission.Granted )
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

                    Sprite sprite = Sprite.Create( _selectedImage, new Rect( 0, 0, _selectedImage.width, _selectedImage.height ), Vector2.zero );

                    _selectedUIImage.sprite = sprite;
                }

                _imageTransfer.SendImageToPeers( _imageData );
            }
        }, fileTypes );

        Debug.Log( "Permission result: " + permission );
    }


    // Example code doesn't use this function but it is here for reference. It's recommended to ask for permissions manually using the
    // RequestPermissionAsync methods prior to calling NativeFilePicker functions
    private async void RequestPermissionAsynchronously( bool readPermissionOnly = false )
    {
        _permission = await NativeFilePicker.RequestPermissionAsync( readPermissionOnly );
        Debug.Log( "Permission result: " + _permission );
    }

    
}