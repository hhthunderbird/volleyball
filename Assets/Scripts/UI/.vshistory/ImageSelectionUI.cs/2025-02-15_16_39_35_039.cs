using System.Collections.Generic;
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

}