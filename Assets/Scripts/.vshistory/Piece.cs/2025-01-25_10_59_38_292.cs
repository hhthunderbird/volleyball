using Fusion;
using UnityEngine;
using UnityEngine.UIElements;

public class Piece : NetworkBehaviour
{
    [Networked] public PlayerRef Owner { get; set; }
    [Networked] public Vector3 OriginalLocation { get; set; }
    [Networked] public float OriginalRotation { get; set; }
    [Networked, OnChangedRender(nameof(BasemapChanged))] public Vector4 BaseMapModifiers { get; set; }
    [Networked, OnChangedRender(nameof(MaskChanged))] public Vector4 MaskModifiers { get; set; }


    [SerializeField] private Texture2D[] _masks;

    private MeshRenderer _renderer;
    public MeshRenderer Renderer => _renderer;

    public override void Spawned()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void BasemapChanged()
    {
        var pBlock = new MaterialPropertyBlock();
        

        pBlock.SetVector( "_BaseMap_ST", BaseMapModifiers );
        pBlock.SetVector( "_Mask_ST", MaskModifiers );

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
    private void MaskChanged()
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere( OriginalLocation, 0.1f );
        Gizmos.DrawLine( transform.position, OriginalLocation );
    }
}
