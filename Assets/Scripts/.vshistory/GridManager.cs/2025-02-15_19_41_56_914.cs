using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class GridManager : NetworkBehaviour
{
    [SerializeField] private Texture2D _mainImage;
    public Texture2D MainImage
    {
        get => _mainImage;
        set => _mainImage = value;
    }

    [SerializeField] private GameObject _piece;

    [SerializeField] private GameObject _bl;
    [SerializeField] private GameObject _l;
    [SerializeField] private GameObject _tl;
    [SerializeField] private GameObject _t;
    [SerializeField] private GameObject _tr;
    [SerializeField] private GameObject _r;
    [SerializeField] private GameObject _br;
    [SerializeField] private GameObject _b;
    [SerializeField] private GameObject _c;

    [SerializeField] private Transform _bgParent;
    [SerializeField] private Transform _piecesParent;

    [SerializeField] private Transform _bg;

    private List<Piece> _pieces = new List<Piece>();
    private int _height;
    private int _width;
    private NetworkVariable<Vector3> _bgScale = new NetworkVariable<Vector3>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public Vector3 BgScale => _bgScale.Value;

    private Vector3 _center;

    public override void OnNetworkSpawn()
    {
        _width = PlayerPrefs.GetInt( PlayerPrefsKeys.Width.ToString() );

        if ( IsSessionOwner )
            BuildGrid();
    }

    private void BuildGrid()
    {
        var horizontalSize = _mainImage.width / ( float ) _width;

        _height = ( int ) ( _mainImage.height / horizontalSize );
        var finalVerticalSize = _mainImage.height / ( float ) _height;
        var verticalScale = finalVerticalSize / horizontalSize;


        var scale = new Vector3( 1, verticalScale, 1 );


        for ( int y = 0; y < _height; y++ )
        {
            for ( int x = 0; x < _width; x++ )
            {
                InstantiateBlock( x, y, scale );
            }
        }

        var xScale = scale.x * _width * 0.5f;
        var yScale = scale.y * _height * 0.5f;
        _bgScale.Value = new Vector3( xScale, yScale, 1 );

        var textureId = PlayerPrefs.GetInt(PlayerPrefsKeys.TextureId.ToString());
        var baseMap = TextureManager.Instance.GetTexture( textureId );

        var bg = Instantiate( _bg );

        bg.GetComponent<Renderer>().sharedMaterial.SetTexture( "_BaseMap", baseMap );
        bg.position = _center;
        bg.localScale = _bgScale.Value;

        bg.GetComponent<NetworkObject>().Spawn();
    }

    private async void InstantiateBlock( int x, int y, Vector2 scale )
    {
        var horizontalStart = ( _width * scale.x * 0.25f ) - ( scale.x * 0.25f );
        var verticalStart = ( _height * scale.y * 0.25f ) - ( scale.y * 0.25f );
        var start = new Vector3( horizontalStart, verticalStart, 0 );
        var position = new Vector3( x * scale.x * 0.5f, y * scale.y * 0.5f, 0 );
        position -= start;
        _center += position / (_width * _height);

        var hOffsetStart = 1f / _width / -2f;
        var vOffsetStart = 1f / _height / -2f;

        var hOffset = 1f / _width;
        var vOffset = 1f / _height;

        var vTiling = 1f / _height * 2;
        var hTiling = 1f / _width * 2;

        var tiling = new Vector2( hTiling, vTiling );
        var offset = new Vector2( hOffsetStart + ( x * hOffset ), vOffsetStart + ( y * vOffset ) );

        GameObject obj = null;

        if ( x == 0 && y == 0 )
            obj = Instantiate( _bl, position, Quaternion.identity, null );

        if ( x == 0 && y > 0 && y < _height - 1 )
            obj = Instantiate( _l, position, Quaternion.identity, null );

        if ( x == 0 && y == _height - 1 )
            obj = Instantiate( _tl, position, Quaternion.identity, null );

        if ( x > 0 && x < _width - 1 && y == _height - 1 )
            obj = Instantiate( _t, position, Quaternion.identity, null );

        if ( x == _width - 1 && y == _height - 1 )
            obj = Instantiate( _tr, position, Quaternion.identity, null );

        if ( x == _width - 1 && y > 0 && y < _height - 1 )
            obj = Instantiate( _r, position, Quaternion.identity, null );

        if ( x == _width - 1 && y == 0 )
            obj = Instantiate( _br, position, Quaternion.identity, null );

        if ( x > 0 && x < _width - 1 && y == 0 )
            obj = Instantiate( _b, position, Quaternion.identity, null );

        if ( x > 0 && x < _width - 1 && y > 0 && y < _height - 1 )
            obj = Instantiate( _c, position, Quaternion.identity, null );



        obj.transform.position = position;
        obj.transform.localScale = scale;

        var piece = obj.GetComponent<Piece>();
        _pieces.Add( piece );
        
        var ngo = obj.GetComponent<NetworkObject>();

        ngo.Spawn();

        await UniTask.WaitUntil( () => ngo.IsSpawned );

        piece.OriginalPosition = position;
        piece.TextureId = PlayerPrefs.GetInt(PlayerPrefsKeys.TextureId.ToString());
        piece.BaseMapModifiers = new Vector4( tiling.x, tiling.y, offset.x, offset.y );
        
    }
}
