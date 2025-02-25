using Fusion;
using UnityEngine;

public class Piece : NetworkBehaviour
{
    [Networked] public PlayerRef Owner { get; set; }
    [Networked] public Vector3 OriginalLocation { get; set; }
    [Networked] public float OriginalRotation { get; set; }
    
    [Networked, OnChangedRender( nameof( BasemapChanged ) )] 
    public int MainTextureIndex { get; set; }
    
    [Networked, OnChangedRender( nameof( MaskChanged ) )] 
    public TileMask MaskTextureIndex { get; set; }
    
    [Networked, OnChangedRender( nameof( BasemapModifsChanged ) )] 
    public Vector4 BaseMapModifiers { get; set; }
    
    [Networked, OnChangedRender( nameof( MaskModifsChanged ) )] 
    public Vector4 MaskModifiers { get; set; }

    private MeshRenderer _renderer;
    //public MeshRenderer Renderer => _renderer;

    //public Texture2D BaseMap { get; set; }
    //public Texture2D MaskMap { get; set; }

    public override void Spawned()
    {
        //_changeDetector = GetChangeDetector( ChangeDetector.Source.SimulationState );
        _renderer = GetComponent<MeshRenderer>();
    }

    private void BasemapChanged()
    {
        var baseMap = TextureManager.Instance.GetTexture( MainTextureIndex );
        var pBlock = new MaterialPropertyBlock();
        pBlock.SetTexture( "_BaseMap", baseMap );
        _renderer.SetPropertyBlock( pBlock );
    }
    private void MaskChanged()
    {
        var maskMap = TextureManager.Instance.GetMask( MaskTextureIndex );
        var pBlock = new MaterialPropertyBlock();
        pBlock.SetTexture( "_Mask", maskMap );
        _renderer.SetPropertyBlock( pBlock );
    }

    private void BasemapModifsChanged()
    {
        var pBlock = new MaterialPropertyBlock();
        pBlock.SetVector( "_BaseMap_ST", BaseMapModifiers );
        _renderer.SetPropertyBlock( pBlock );
    }
    private void MaskModifsChanged()
    {
        var pBlock = new MaterialPropertyBlock();
        pBlock.SetVector( "_Mask_ST", MaskModifiers );
        _renderer.SetPropertyBlock( pBlock );
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere( OriginalLocation, 0.1f );
        Gizmos.DrawLine( transform.position, OriginalLocation );
    }
}
