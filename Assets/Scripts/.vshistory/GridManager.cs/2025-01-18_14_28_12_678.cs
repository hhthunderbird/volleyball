using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _pieces;

    [SerializeField] private int _height;
    [SerializeField] private int _length;
    [SerializeField] private int _sideSize;

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
        var size = _pieces[ 0 ].GetComponent<SpriteRenderer>().size.x;

        for ( int l = 0; l < _length; l++ )
        {
            for ( int h = 0; h < _height; h++ )
            {

            }
        }
    }
}
