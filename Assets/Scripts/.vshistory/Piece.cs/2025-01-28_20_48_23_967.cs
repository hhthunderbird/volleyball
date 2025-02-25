using Fusion;
using UnityEngine;


public class Piece : NetworkBehaviour
{
    [Networked] public PlayerRef Owner { get; set; }
    [Networked] public bool IsBeingDragged { get; set; }
    [Networked] public Vector3 Position { get; set; }
    [Networked] public Quaternion Rotation { get; set; }
    [Networked] public Vector3 OriginalPosition { get; set; }
    [Networked] public float OriginalRotation { get; set; }
    [Networked] public TileMask MaskTextureIndex { get; set; } = TileMask.C;
    [Networked] public int MainTextureIndex { get; set; } = -1;
    [Networked] public Vector4 BaseMapModifiers { get; set; }
    [Networked] public Vector4 MaskModifiers { get; set; }

    private MeshRenderer _renderer;

    private ChangeDetector _changeDetector;

    public override void Spawned()
    {
        //_changeDetector = GetChangeDetector( ChangeDetector.Source.SimulationState );
        _renderer = GetComponent<MeshRenderer>();

        RefreshState();
    }

    //public override void FixedUpdateNetwork()
    //{
    //    if ( !HasInputAuthority && IsBeingDragged )
    //    {
    //        var screenPos = Input.mousePosition;
    //        screenPos.z = -Camera.main.transform.position.z;
    //        Position = Camera.main.ScreenToWorldPoint( screenPos );
    //        transform.SetPositionAndRotation( Position, Rotation );
    //    }
    //}

    //public override void Render()
    //{
    //    var pBlock = new MaterialPropertyBlock();
    //    foreach ( var change in _changeDetector.DetectChanges( this ) )
    //    {
    //        switch ( change )
    //        {
    //            case nameof( MainTextureIndex ):
    //                var baseMap = TextureManager.Instance.GetTexture( MainTextureIndex );
    //                pBlock.SetTexture( "_BaseMap", baseMap );
    //                _renderer.SetPropertyBlock( pBlock );
    //                break;
    //            case nameof( MaskTextureIndex ):
    //                var maskMap = TextureManager.Instance.GetMask( MaskTextureIndex );

    //                pBlock.SetTexture( "_Mask", maskMap );
    //                _renderer.SetPropertyBlock( pBlock );
    //                break;
    //            case nameof( BaseMapModifiers ):
    //                pBlock.SetVector( "_BaseMap_ST", BaseMapModifiers );
    //                _renderer.SetPropertyBlock( pBlock );
    //                break;
    //            case nameof( MaskModifiers ):
    //                pBlock.SetVector( "_Mask_ST", MaskModifiers );
    //                _renderer.SetPropertyBlock( pBlock );
    //                break;
    //            case nameof( Position ):
    //                Debug.Log( "asçdlfkjasçdflkjasd" );
    //                transform.position = OriginalPosition;
    //                break;
    //            default:
    //                break;
    //        }
    //    }
    //}

    private void RefreshState()
    {
        //Debug.LogWarning( "aaaaaaaaa" );
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
        //Gizmos.DrawSphere( OriginalPosition, 0.1f );

        if ( Vector3.Distance( transform.position, OriginalPosition ) <= 1 )
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawLine( transform.position, OriginalPosition );
    }
}
