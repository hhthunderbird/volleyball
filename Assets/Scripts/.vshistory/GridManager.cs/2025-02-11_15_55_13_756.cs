using Unity.Netcode;
using System.Collections.Generic;
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

    private List<Piece> _pieces = new List<Piece>();
    public List<Piece> Pieces => _pieces;
    private int _height;
    private int _width;

    public override void OnNetworkSpawn()
    {
        _width = PlayerPrefs.GetInt( PlayerPrefsKeys.Width.ToString() );
        if ( IsSessionOwner )
            BuildGrid();
    }

    private void BuildGrid()
    {
        _width = 5;
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
    }

    private void InstantiateBlock( int x, int y, Vector2 scale )
    {
        var position = new Vector3( x * scale.x * 0.5f, y * scale.y * 0.5f, 0 );

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

        piece.OriginalPosition = position;
        piece.MainTextureIndex = 0;
        piece.BaseMapModifiers = new Vector4( tiling.x, tiling.y, offset.x, offset.y );
        piece.MaskModifiers = new Vector4( 1, 1, 0, 0 );
        obj.GetComponent<NetworkObject>().Spawn();
        //piece.MaskTextureIndex = mask;
    }

    private void ApplySprite( int x, int y, Piece piece )
    {
        var hOffsetStart = 1f / _width / -2f;
        var vOffsetStart = 1f / _height / -2f;

        var hOffset = 1f / _width;
        var vOffset = 1f / _height;

        var vTiling = 1f / _height * 2;
        var hTiling = 1f / _width * 2;

        var tiling = new Vector2( hTiling, vTiling );
        var offset = new Vector2( hOffsetStart + ( x * hOffset ), vOffsetStart + ( y * vOffset ) );

        var mask = TileMask.C;

        if ( x == 0 && y == 0 )
            mask = TileMask.BL;

        if ( x == 0 && y > 0 && y < _height - 1 )
            mask = TileMask.L;

        if ( x == 0 && y == _height - 1 )
            mask = TileMask.TL;

        if ( x > 0 && x < _width - 1 && y == _height - 1 )
            mask = TileMask.T;

        if ( x == _width - 1 && y == _height - 1 )
            mask = TileMask.TR;

        if ( x == _width - 1 && y > 0 && y < _height - 1 )
            mask = TileMask.R;

        if ( x == _width - 1 && y == 0 )
            mask = TileMask.BR;

        if ( x > 0 && x < _width - 1 && y == 0 )
            mask = TileMask.B;

        piece.MainTextureIndex = 0;
        piece.BaseMapModifiers = new Vector4( tiling.x, tiling.y, offset.x, offset.y );
        piece.MaskModifiers = new Vector4( 1, 1, 0, 0 );
        piece.MaskTextureIndex = mask;
    }
}
