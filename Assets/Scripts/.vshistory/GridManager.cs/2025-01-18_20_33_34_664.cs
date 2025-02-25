using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GridManager : MonoBehaviour
{
    [SerializeField] private List<Piece> _pieces;
    [SerializeField] private List<Transform> _bgPieces;

    private Transform[,] _piecesGrid;

    [SerializeField] private int _height;
    [SerializeField] private int _length;
    [SerializeField] private float _sideSize;

    public bool _go;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ( _go )
        {
            _go = false;
            Do();
        }
    }

    private void Do()
    {
        _sideSize = _bgPieces[ 0 ].GetComponent<SpriteRenderer>().size.x;

        _piecesGrid = new Transform[ _length, _height ];

        int index = 0;

        var pBlock = new MaterialPropertyBlock();

        for ( int h = 0; h < _height; h++ )
        {
            for ( int l = 0; l < _length; l++ )
            {
                var z = _bgPieces[ index ].position.z;
                var position = new Vector3( l * _sideSize, h * _sideSize, z );
                _bgPieces[ index ].position = _pieces[ index ].transform.position = position;

                _pieces[ index ].OriginalLocation = new Vector3( l * _sideSize, h * _sideSize, z );

                var renderer = _pieces[ index ].GetComponent<SpriteRenderer>();

                var horizontalTiling = 1f / _length;
                var verticalTiling = 1f / _height;

                var offset = new Vector2( h * horizontalTiling, l * verticalTiling );
                var tiling = new Vector2( horizontalTiling, verticalTiling );

                pBlock.SetVector( "_Offset", offset );
                pBlock.SetVector( "_Tiling", offset );
                renderer.SetPropertyBlock( pBlock );

#if UNITY_EDITOR
                EditorUtility.SetDirty( _pieces[ index ] );
#endif

                _piecesGrid[ l, h ] = _bgPieces[ index ];
                index++;
            }
        }
    }
}
