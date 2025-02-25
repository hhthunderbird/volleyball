using Fusion;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    TL,T,TR,R,BR,B,BL,L,C
}

public class Piece : NetworkBehaviour
{
    [Networked] public PlayerRef Owner { get; set; }
    [Networked] public Vector3 OriginalLocation { get; set; }
    [Networked] public float OriginalRotation { get; set; }
    [Networked] public TileType TileType { get; set; }
    [Networked, OnChangedRender(nameof(BasemapChanged))] public Vector4 BaseMapModifiers { get; set; }
    [Networked, OnChangedRender(nameof(MaskChanged))] public Vector4 MaskModifiers { get; set; }

    public Dictionary<TileType,Texture2D> MaskTiles = new Dictionary<TileType, Texture2D>();

    private MeshRenderer _renderer;
    public MeshRenderer Renderer => _renderer;

    public override void Spawned()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void BasemapChanged()
    {
        var pBlock = new MaterialPropertyBlock();
        //pBlock.SetTexture( "_BaseMap", _mainImage );
        

        pBlock.SetVector( "_BaseMap_ST", BaseMapModifiers );
        pBlock.SetVector( "_Mask_ST", MaskModifiers );

        

        Renderer.SetPropertyBlock( pBlock );
    }
    private void MaskChanged()
    {
        var pBlock = new MaterialPropertyBlock();

        var tex = MaskTiles.GetValueOrDefault( TileType );

        pBlock.SetTexture( "_Mask", tex );

        Renderer.SetPropertyBlock( pBlock );
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere( OriginalLocation, 0.1f );
        Gizmos.DrawLine( transform.position, OriginalLocation );
    }
}
