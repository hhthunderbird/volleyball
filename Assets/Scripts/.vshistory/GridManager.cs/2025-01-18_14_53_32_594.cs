using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridManager : MonoBehaviour
{
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

        for ( int l = 0; l < _length; l++ )
        {
            for ( int h = 0; h < _height; h++ )
            {
                var z = _bgPieces[ index ].position.z;
                var position = new Vector3( l * _sideSize, h * _sideSize, z );
                _bgPieces[ index ].position = position;
                _piecesGrid[ l, h ] = _bgPieces[ index ];
                index++;
            }
        }
    }
}
