using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageSelectionUI : MonoBehaviour
{
    [SerializeField] private List<Texture2D> _images;
    [SerializeField] private Texture2D _selectedImage;

    [SerializeField] private Text _lobbyName;
    [SerializeField] private Button _lessPiecesButton;
    [SerializeField] private Button _morePiecesButton;
    [SerializeField] private Text _totalPieces;

    private int _minLength = 3;
    private int _maxLength = 100;

    private int _width;
    private int _textureId;


    private string _pngFileType;
    private string _jpgFileType;

    private void Awake()
    {
        PlayerPrefs.SetString( PlayerPrefsKeys.RoomName.ToString(), "" );
        PlayerPrefs.Save();
    }

    private void Start()
    {
        _pngFileType = NativeFilePicker.ConvertExtensionToFileType( "png" ); // Returns "application/pdf" on Android and "com.adobe.pdf" on iOS
        _jpgFileType = NativeFilePicker.ConvertExtensionToFileType( "jpg" ); // Returns "application/pdf" on Android and "com.adobe.pdf" on iOS
        
        Debug.Log( "pdf's MIME/UTI is: " + _pngFileType );

        _lessPiecesButton.onClick.AddListener( LessPiecesButtonListener );
        _morePiecesButton.onClick.AddListener( MorePiecesButtonListener );
        
        _width = 3;
        _totalPieces.text = TotalPieces();
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

    private void OpenFileBrowser()
    {
        // Don't attempt to import/export files if the file picker is already open
        if ( NativeFilePicker.IsFilePickerBusy() )
            return;

        if ( Input.mousePosition.x < Screen.width / 3 )
        {
            // Pick a PDF file
            NativeFilePicker.Permission permission = NativeFilePicker.PickFile( ( path ) =>
            {
                if ( path == null )
                    Debug.Log( "Operation cancelled" );
                else
                    Debug.Log( "Picked file: " + path );
            }, new string[] { _pngFileType } );

            Debug.Log( "Permission result: " + permission );
        }
        else if ( Input.mousePosition.x < Screen.width * 2 / 3 )
        {
#if UNITY_ANDROID
            // Use MIMEs on Android
            string[] fileTypes = new string[] { "image/*", "video/*" };
#else
			// Use UTIs on iOS
			string[] fileTypes = new string[] { "public.image", "public.movie" };
#endif

            // Pick image(s) and/or video(s)
            NativeFilePicker.Permission permission = NativeFilePicker.PickMultipleFiles( ( paths ) =>
            {
                if ( paths == null )
                    Debug.Log( "Operation cancelled" );
                else
                {
                    for ( int i = 0; i < paths.Length; i++ )
                        Debug.Log( "Picked file: " + paths[ i ] );
                }
            }, fileTypes );

            Debug.Log( "Permission result: " + permission );
        }
        else
        {
            // Create a dummy text file
            string filePath = Path.Combine( Application.temporaryCachePath, "test.txt" );
            File.WriteAllText( filePath, "Hello world!" );

            // Export the file
            NativeFilePicker.Permission permission = NativeFilePicker.ExportFile( filePath, ( success ) => Debug.Log( "File exported: " + success ) );

            Debug.Log( "Permission result: " + permission );
        }
    }

}