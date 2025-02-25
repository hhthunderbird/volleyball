using Fusion;
using UnityEngine;


public class Piece : NetworkBehaviour
{
    [Networked] public PlayerRef Owner { get; set; }
    [Networked] public Vector3 OriginalLocation { get; set; }
    [Networked] public float OriginalRotation { get; set; }
    [Networked] public TileMask MaskTextureIndex { get; set; } = TileMask.C;
    [Networked] public int MainTextureIndex { get; set; } = -1;
    [Networked] public Vector4 BaseMapModifiers { get; set; }
    [Networked] public Vector4 MaskModifiers { get; set; }

    private MeshRenderer _renderer;

    private ChangeDetector _changeDetector;

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector( ChangeDetector.Source.SimulationState );
        _renderer = GetComponent<MeshRenderer>();
    }

    public override void Render()
    {
        var pBlock = new MaterialPropertyBlock();
        foreach ( var change in _changeDetector.DetectChanges( this ) )
        {
            switch ( change )
            {
                case nameof( MainTextureIndex ):
                    Debug.Log( "BASEMAP !!!!!!!!!!!!!!!!!!!!" );
                    var baseMap = TextureManager.Instance.GetTexture( MainTextureIndex );
                    pBlock.SetTexture( "_BaseMap", baseMap );
                    _renderer.SetPropertyBlock( pBlock );
                    break;
                case nameof( MaskTextureIndex ):
                    Debug.Log( "MASK" );
                    var maskMap = TextureManager.Instance.GetMask( MaskTextureIndex );

                    pBlock.SetTexture( "_Mask", maskMap );
                    _renderer.SetPropertyBlock( pBlock );
                    break;
                case nameof( BaseMapModifiers ):
                    Debug.Log( "BASEMAP MODIFS" );

                    pBlock.SetVector( "_BaseMap_ST", BaseMapModifiers );
                    _renderer.SetPropertyBlock( pBlock );
                    break;
                case nameof( MaskModifiers ):
                    Debug.Log( "MASK MODIFS" );

                    pBlock.SetVector( "_Mask_ST", MaskModifiers );
                    _renderer.SetPropertyBlock( pBlock );
                    break;
                default:
                    break;
            }
        }
    }

    [Rpc( RpcSources.All, RpcTargets.StateAuthority )]
    public void RefreshStateRpc()
    {
        Debug.LogWarning( "aaaaaaaaa" );
        var pBlock = new MaterialPropertyBlock();

        var baseMap = TextureManager.Instance.GetTexture( MainTextureIndex );
        var maskMap = TextureManager.Instance.GetMask( MaskTextureIndex );

        pBlock.SetTexture( "_BaseMap", baseMap );

        pBlock.SetTexture( "_Mask", maskMap );

        pBlock.SetVector( "_BaseMap_ST", BaseMapModifiers );

        pBlock.SetVector( "_Mask_ST", MaskModifiers );
        
        _renderer.SetPropertyBlock( pBlock );
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere( OriginalLocation, 0.1f );
        Gizmos.DrawLine( transform.position, OriginalLocation );
    }
}
