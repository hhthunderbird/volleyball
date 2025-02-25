using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Piece _piece;
    [SerializeField] private Transform _bgPiece;

    //private Transform[,] _piecesGrid;

    [SerializeField] private Sprite _topLeftCorner;
    [SerializeField] private Sprite _topRightCorner;
    [SerializeField] private Sprite _bottomLeftCorner;
    [SerializeField] private Sprite _bottomRightCorner;

    [SerializeField] private Sprite _bottom;
    [SerializeField] private Sprite _top;
    [SerializeField] private Sprite _left;
    [SerializeField] private Sprite _right;

    [SerializeField] private Sprite _center;

    [SerializeField] private int _height;
    [SerializeField] private int _length;
    [SerializeField] private float _sideSize;

    void Start()
    {
        Do();
    }

    private void Do()
    {
        _sideSize = _bgPiece.GetComponent<SpriteRenderer>().size.x;

        //_piecesGrid = new Transform[ _length, _height ];

        //int index = 0;

        var pBlock = new MaterialPropertyBlock();

        for ( int y = 0; y < _height; y++ )
        {
            for ( int x = 0; x < _length; x++ )
            {
                var bgPiece = Instantiate( _bgPiece );

                var piece = Instantiate( _piece );

                var z = bgPiece.position.z;
                var position = new Vector3( x * _sideSize, y * _sideSize, z );
                bgPiece.position = piece.transform.position = position;

                piece.OriginalLocation = new Vector3( x * _sideSize, y * _sideSize, z );

                var renderer = piece.Renderer;

                var horizontalTiling = 1f / ( _length + 1 );
                var verticalTiling = 1f / ( _height + 1 );

                var offset = new Vector2( x * horizontalTiling, y * verticalTiling );
                var tiling = new Vector2( 1f / 2, 1f / 2 );

                pBlock.SetVector( "_Offset", offset );
                pBlock.SetVector( "_Tiling", tiling );
                renderer.SetPropertyBlock( pBlock );


                ApplySprite( x, y, piece );


                //_piecesGrid[ l, h ] = bgPiece;
                //index++;
            }
        }
    }

    private void ApplySprite(int x, int y, Piece piece )
    {
        if(x == 0 && y == 0 )
        {
            piece.Renderer.sprite = _topLeftCorner;
        }
    }


    //private void Clear()
    //{
    //    var oldTiles = FindObjectsByType<SpriteRenderer>( FindObjectsSortMode.None );

    //    for ( int i = 0; i < oldTiles.Length; i++ )
    //        DestroyImmediate( oldTiles[ i ] );
    //}
}
