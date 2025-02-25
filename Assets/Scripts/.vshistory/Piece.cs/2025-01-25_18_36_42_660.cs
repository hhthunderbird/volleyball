using Fusion;
using UnityEngine;

public class Piece : NetworkBehaviour
{
    [Networked] public PlayerRef Owner { get; set; }
    [Networked] public Vector3 OriginalLocation { get; set; }
    [Networked] public float OriginalRotation { get; set; }
    [Networked] public int MainTextureIndex { get; set; }
    [Networked] public TileMask MaskTextureIndex { get; set; }

    [Networked/*, OnChangedRender( nameof( BasemapModifsChanged ) )*/]
    public Vector4 BaseMapModifiers { get; set; }

    [Networked/*, OnChangedRender( nameof( MaskModifsChanged ) )*/]
    public Vector4 MaskModifiers { get; set; }

    private MeshRenderer _renderer;

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
                case nameof( MainTextureIndex ):
                    Debug.Log( "BASEMAP" );
                    break;
                case nameof( MaskTextureIndex ):
                    Debug.Log( "MASK" );
                    break;
                default:
                    break;
            }
        }
    }

    private void BasemapChanged()
    {
        Debug.Log( "BASEMAP" );
        var baseMap = TextureManager.Instance.GetTexture( MainTextureIndex );
        //var pBlock = new MaterialPropertyBlock();
        //pBlock.SetTexture( "_BaseMap", baseMap );
        //_renderer.SetPropertyBlock( pBlock );
        _renderer.sharedMaterial.SetTexture( "_BaseMap", baseMap );
    }
    private void MaskChanged()
    {
        Debug.Log( "MASK" );
        var maskMap = TextureManager.Instance.GetMask( MaskTextureIndex );
        //var pBlock = new MaterialPropertyBlock();
        //pBlock.SetTexture( "_Mask", maskMap );
        //_renderer.SetPropertyBlock( pBlock );
        _renderer.sharedMaterial.SetTexture( "_Mask", maskMap );
    }

    private void BasemapModifsChanged()
    {
        Debug.Log( "BASEMAP VALUES" );
        var pBlock = new MaterialPropertyBlock();
        pBlock.SetVector( "_BaseMap_ST", BaseMapModifiers );
        _renderer.SetPropertyBlock( pBlock );
    }
    private void MaskModifsChanged()
    {
        Debug.Log( "MASK VALUES" );
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
