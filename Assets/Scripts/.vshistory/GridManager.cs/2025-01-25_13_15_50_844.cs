using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : NetworkBehaviour
{
    [SerializeField] private Texture2D _mainImage;
    
    //[SerializeField] private Piece _piece;
    //[SerializeField] private Transform _bgPiece;

    [SerializeField] private NetworkPrefabRef _piece;
    [SerializeField] private NetworkPrefabRef _bgPiece;

    [SerializeField] private Transform _bgParent;
    [SerializeField] private Transform _piecesParent;

    [SerializeField] private Texture2D _topLeftCorner;
    [SerializeField] private Texture2D _topRightCorner;
    [SerializeField] private Texture2D _bottomLeftCorner;
    [SerializeField] private Texture2D _bottomRightCorner;

    [SerializeField] private Texture2D _bottom;
    [SerializeField] private Texture2D _top;
    [SerializeField] private Texture2D _left;
    [SerializeField] private Texture2D _right;

    [SerializeField] private Texture2D _center;

    [SerializeField] private int _height;
    [SerializeField] private int _length;
    [SerializeField] private float _xScale = 1f;
    [SerializeField] private float _yScale = 1f;

    void Start()
    {
        Do();
    }

    private void Do()
    {
        if ( !Runner.IsServer ) return;

        if ( _mainImage.width > _mainImage.height )
        {
            Debug.Log( "length is higher" );
            _xScale = ( float ) _mainImage.width / _mainImage.height;
        }
        else if ( _mainImage.height > _mainImage.width )
        {
            Debug.Log( "height is higher" );
            _yScale = (float)_mainImage.height / _mainImage.width;
        }

        //_piecesGrid = new Transform[ _length, _height ];

        //int index = 0;

        for ( int y = 0; y < _height; y++ )
        {
            for ( int x = 0; x < _length; x++ )
            {
                //var bgPiece = Instantiate( _bgPiece, _bgParent );
                //bgPiece.transform.localScale = new Vector3( _xScale, _yScale, 1 );

                //var piece = Instantiate( _piece, _piecesParent );
                //piece.transform.localScale = new Vector3( _xScale, _yScale, 1 );

                //var z = bgPiece.position.z;
                //var position = new Vector3( x * _xScale * 0.5f, y * _yScale * 0.5f, z );
                //bgPiece.position = piece.transform.position = position;

                //piece.OriginalLocation = new Vector3( x * _xScale * 0.5f, y * _yScale * 0.5f, z );
                //piece.OriginalRotation = Quaternion.identity.eulerAngles.z;


                var pieceScale = new Vector3( _xScale, _yScale, 1 );
                var bgPieceScale = new Vector3( _xScale, _yScale, 1 );

                //var z = bgPiece.position.z;
                var position = new Vector3( x * _xScale * 0.5f, y * _yScale * 0.5f, 0 );

                var piece = Runner.Spawn( _piece, position, Quaternion.identity, null  );
                
                piece.transform.localScale = pieceScale;

                var pieceComponent = piece.GetComponent<Piece>();

                pieceComponent.OriginalLocation = new Vector3( x * _xScale * 0.5f, y * _yScale * 0.5f, 0 );
                pieceComponent.OriginalRotation = Quaternion.identity.eulerAngles.z;

                ApplySprite( x, y, pieceComponent );

                //_piecesGrid[ l, h ] = bgPiece;
                //index++;
            }
        }
    }

    private void ApplySprite( int x, int y, Piece piece )
    {
        var pBlock = new MaterialPropertyBlock();
        pBlock.SetTexture( "_BaseMap", _mainImage );

        var hOffsetStart = 1f / _length / -2f;
        var vOffsetStart = 1f / _height / -2f;

        var hOffset = 1f / _length;
        var vOffset = 1f / _height;

        var vTiling = 1f / _height * 2;
        var hTiling = 1f / _length * 2;

        var tiling = new Vector2( hTiling, vTiling );
        var offset = new Vector2( hOffsetStart + x * hOffset, vOffsetStart + y * vOffset );

        pBlock.SetVector( "_BaseMap_ST", new Vector4( tiling.x, tiling.y, offset.x, offset.y ) );
        pBlock.SetVector( "_Mask_ST", new Vector4( 1, 1, 0, 0 ) );

        var tex = _center;

        if ( x == 0 && y == 0 )
            tex = _bottomLeftCorner;

        if ( x == 0 && y > 0 && y < _height - 1 )
            tex = _left;

        if ( x == 0 && y == _height - 1 )
            tex = _topLeftCorner;

        if ( x > 0 && x < _length - 1 && y == _height - 1 )
            tex = _top;

        if ( x == _length - 1 && y == _height - 1 )
            tex = _topRightCorner;

        if ( x == _length - 1 && y > 0 && y < _height - 1 )
            tex = _right;

        if ( x == _length - 1 && y == 0 )
            tex = _bottomRightCorner;

        if ( x > 0 && x < _length - 1 && y == 0 )
            tex = _bottom;

        pBlock.SetTexture( "_Mask", tex );

        piece.Renderer.SetPropertyBlock( pBlock );
    }


    //private void Clear()
    //{
    //    var oldTiles = FindObjectsByType<SpriteRenderer>( FindObjectsSortMode.None );

    //    for ( int i = 0; i < oldTiles.Length; i++ )
    //        DestroyImmediate( oldTiles[ i ] );
    //}
}
