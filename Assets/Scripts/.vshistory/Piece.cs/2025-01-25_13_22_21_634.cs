using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class Piece : NetworkBehaviour
{
    [Networked] public PlayerRef Owner { get; set; }
    [Networked] public Vector3 OriginalLocation { get; set; }
    [Networked] public float OriginalRotation { get; set; }
    [Networked, OnChangedRender(nameof(BasemapChanged))] public Vector4 BaseMapModifiers { get; set; }
    [Networked, OnChangedRender(nameof(MaskChanged))] public Vector4 MaskModifiers { get; set; }

    private MeshRenderer _renderer;
    public MeshRenderer Renderer => _renderer;

    /*[Networked] */public Texture2D BaseMap { get; set; }
    /*[Networked] */public Texture2D MaskMap { get; set; }

    private ChangeDetector _changeDetector;

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector( ChangeDetector.Source.SimulationState );
        _renderer = GetComponent<MeshRenderer>();
    }

    public override void Render()
    {
        foreach ( var change in _changeDetector.DetectChanges( this ) )
        {
            switch ( change )
            {
                case nameof( BaseMap ):
                    var bmBlock = new MaterialPropertyBlock();
                    bmBlock.SetTexture( "_BaseMap", BaseMap );
                    Renderer.SetPropertyBlock( bmBlock );
                    break;
                case nameof( MaskMap ):
                    var mBlock = new MaterialPropertyBlock();
                    mBlock.SetTexture( "_Mask", MaskMap );
                    Renderer.SetPropertyBlock( mBlock );
                    break;
                default:
                    break;
            }
        }
    }

    private void BasemapChanged()
    {
        var pBlock = new MaterialPropertyBlock();
        pBlock.SetVector( "_BaseMap_ST", BaseMapModifiers );
        Renderer.SetPropertyBlock( pBlock );
    }
    private void MaskChanged()
    {
        var pBlock = new MaterialPropertyBlock();
        pBlock.SetVector( "_Mask_ST", MaskModifiers );
        Renderer.SetPropertyBlock( pBlock );
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere( OriginalLocation, 0.1f );
        Gizmos.DrawLine( transform.position, OriginalLocation );
    }
}
