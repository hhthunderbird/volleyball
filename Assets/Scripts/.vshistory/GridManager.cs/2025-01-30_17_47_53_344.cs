using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : NetworkBehaviour
{
    [SerializeField] private Texture2D _mainImage;

    [SerializeField] private NetworkPrefabRef _piece;
    
    
    [SerializeField] private NetworkPrefabRef _bl;
    [SerializeField] private NetworkPrefabRef _l;
    [SerializeField] private NetworkPrefabRef _tl;
    [SerializeField] private NetworkPrefabRef _t;
    [SerializeField] private NetworkPrefabRef _tr;
    [SerializeField] private NetworkPrefabRef _r;
    [SerializeField] private NetworkPrefabRef _br;
    [SerializeField] private NetworkPrefabRef _b;
    [SerializeField] private NetworkPrefabRef _c;

    [SerializeField] private Transform _bgParent;
    [SerializeField] private Transform _piecesParent;

    private List<Piece> _pieces = new List<Piece>();
    public List<Piece> Pieces => _pieces;

    [SerializeField] private int _height;
    [SerializeField] private int _length;
    [SerializeField] private float _xScale = 1f;
    [SerializeField] private float _yScale = 1f;

    public override void Spawned()
    {
        BuildGrid();
    }

    private void BuildGrid()
    {
        if ( Runner.LocalPlayer.PlayerId > 1 ) return;

        if ( _mainImage.width > _mainImage.height )
        {
            _xScale = ( float ) _mainImage.width / _mainImage.height;
        }
        else if ( _mainImage.height > _mainImage.width )
        {
            _yScale = ( float ) _mainImage.height / _mainImage.width;
        }

        for ( int y = 0; y < _height; y++ )
        {
            for ( int x = 0; x < _length; x++ )
            {
                InstantiateBlock( x, y );
                var pieceScale = new Vector3( _xScale, _yScale, 1 );
                var bgPieceScale = new Vector3( _xScale, _yScale, 1 );

                var position = new Vector3( x * _xScale * 0.5f, y * _yScale * 0.5f, 0 );

                var piece = Runner.Spawn( _piece, position, Quaternion.identity, null );

                piece.transform.localScale = pieceScale;

                var pieceComponent = piece.GetComponent<Piece>();

                pieceComponent.OriginalPosition = new Vector3( x * _xScale * 0.5f, y * _yScale * 0.5f, 0 );
                pieceComponent.OriginalRotation = Quaternion.identity.eulerAngles.z;

                _pieces.Add( pieceComponent );

                ApplySprite( x, y, pieceComponent );
            }
        }
    }

    private void InstantiateBlock(int x, int y )
    {
        var scale = new Vector3( _xScale, _yScale, 1 );
        //var bgPieceScale = new Vector3( _xScale, _yScale, 1 );

        var position = new Vector3( x * _xScale * 0.5f, y * _yScale * 0.5f, 0 );

        //var piece = Runner.Spawn( _piece, position, Quaternion.identity, null );

        //piece.transform.localScale = pieceScale;

        //var pieceComponent = piece.GetComponent<Piece>();

        //pieceComponent.OriginalPosition = new Vector3( x * _xScale * 0.5f, y * _yScale * 0.5f, 0 );
        //pieceComponent.OriginalRotation = Quaternion.identity.eulerAngles.z;

        //_pieces.Add( pieceComponent );


        var hOffsetStart = 1f / _length / -2f;
        var vOffsetStart = 1f / _height / -2f;

        var hOffset = 1f / _length;
        var vOffset = 1f / _height;

        var vTiling = 1f / _height * 2;
        var hTiling = 1f / _length * 2;

        var tiling = new Vector2( hTiling, vTiling );
        var offset = new Vector2( hOffsetStart + ( x * hOffset ), vOffsetStart + ( y * vOffset ) );

        GameObject obj = null;

        if ( x == 0 && y == 0 )
            obj= Runner.Spawn( _bl, position, Quaternion.identity, null ).gameObject;

        if ( x == 0 && y > 0 && y < _height - 1 )
            obj = Runner.Spawn( _l, position, Quaternion.identity, null ).gameObject;

        if ( x == 0 && y == _height - 1 )
            obj = Runner.Spawn( _tl, position, Quaternion.identity, null ).gameObject;

        if ( x > 0 && x < _length - 1 && y == _height - 1 )
            obj = Runner.Spawn( _t, position, Quaternion.identity, null ).gameObject;

        if ( x == _length - 1 && y == _height - 1 )
            obj = Runner.Spawn( _tr, position, Quaternion.identity, null ).gameObject;

        if ( x == _length - 1 && y > 0 && y < _height - 1 )
            obj = Runner.Spawn( _r, position, Quaternion.identity, null ).gameObject;

        if ( x == _length - 1 && y == 0 )
            obj = Runner.Spawn( _br, position, Quaternion.identity, null ).gameObject;

        if ( x > 0 && x < _length - 1 && y == 0 )
            obj = Runner.Spawn( _b, position, Quaternion.identity, null ).gameObject;

        if ( x > 0 && x < _length - 1 && y > 0 && y < _height - 1 )
            obj = Runner.Spawn( _c, position, Quaternion.identity, null ).gameObject;

        //piece.MainTextureIndex = 0;
        //piece.BaseMapModifiers = new Vector4( tiling.x, tiling.y, offset.x, offset.y );
        //piece.MaskModifiers = new Vector4( 1, 1, 0, 0 );
        //piece.MaskTextureIndex = mask;
    }

    private void ApplySprite( int x, int y, Piece piece )
    {
        var hOffsetStart = 1f / _length / -2f;
        var vOffsetStart = 1f / _height / -2f;

        var hOffset = 1f / _length;
        var vOffset = 1f / _height;

        var vTiling = 1f / _height * 2;
        var hTiling = 1f / _length * 2;

        var tiling = new Vector2( hTiling, vTiling );
        var offset = new Vector2( hOffsetStart + ( x * hOffset ), vOffsetStart + ( y * vOffset ) );

        var mask = TileMask.C;

        if ( x == 0 && y == 0 )
            mask = TileMask.BL;

        if ( x == 0 && y > 0 && y < _height - 1 )
            mask = TileMask.L;

        if ( x == 0 && y == _height - 1 )
            mask = TileMask.TL;

        if ( x > 0 && x < _length - 1 && y == _height - 1 )
            mask = TileMask.T;

        if ( x == _length - 1 && y == _height - 1 )
            mask = TileMask.TR;

        if ( x == _length - 1 && y > 0 && y < _height - 1 )
            mask = TileMask.R;

        if ( x == _length - 1 && y == 0 )
            mask = TileMask.BR;

        if ( x > 0 && x < _length - 1 && y == 0 )
            mask = TileMask.B;

        piece.MainTextureIndex = 0;
        piece.BaseMapModifiers = new Vector4( tiling.x, tiling.y, offset.x, offset.y );
        piece.MaskModifiers = new Vector4( 1, 1, 0, 0 );
        piece.MaskTextureIndex = mask;
    }
}
