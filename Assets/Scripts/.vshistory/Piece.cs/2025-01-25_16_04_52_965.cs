using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class Piece : NetworkBehaviour
{
    [Networked] public PlayerRef Owner { get; set; }
    [Networked] public Vector3 OriginalLocation { get; set; }
    [Networked] public float OriginalRotation { get; set; }
    [Networked] public int MainTextureIndex { get; set; }
    [Networked, OnChangedRender(nameof(BasemapModifsChanged))] public Vector4 BaseMapModifiers { get; set; }
    [Networked, OnChangedRender(nameof(MaskModifsChanged))] public Vector4 MaskModifiers { get; set; }

    private MeshRenderer _renderer;
    //public MeshRenderer Renderer => _renderer;

    //public Texture2D BaseMap { get; set; }
    //public Texture2D MaskMap { get; set; }

    public override void Spawned()
    {
        //_changeDetector = GetChangeDetector( ChangeDetector.Source.SimulationState );
        _renderer = GetComponent<MeshRenderer>();
    }

    private void BasemapModifsChanged()
    {
                    //Debug.Log( "BASEMAP!!!!!!!!!!!!!!" );
        var pBlock = new MaterialPropertyBlock();
        pBlock.SetTexture( "_BaseMap", BaseMap );
        pBlock.SetVector( "_BaseMap_ST", BaseMapModifiers );
        Renderer.SetPropertyBlock( pBlock );
    }
    private void MaskModifsChanged()
    {
                    //Debug.Log( "MASK!!!!!!!!!!!!!!" );
        var pBlock = new MaterialPropertyBlock();
        pBlock.SetTexture( "_Mask", MaskMap );
        pBlock.SetVector( "_Mask_ST", MaskModifiers );
        Renderer.SetPropertyBlock( pBlock );
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere( OriginalLocation, 0.1f );
        Gizmos.DrawLine( transform.position, OriginalLocation );
    }
}
