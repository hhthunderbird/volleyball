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
        for ( int i = 0; i < _height; i++ )
        {
            for ( int j = 0; j < _length; j++ )
            {

            }
        }
    }
}
