using Fusion;
using UnityEngine;


public class Piece : NetworkBehaviour
{
    public PlayerInputPoll PlayerInputPoll;// { get; set; }
    public Camera Camera { get; set; }
    [Networked] public PlayerRef Owner { get; set; }
    [Networked] public bool IsBeingDragged { get; set; }
    [Networked] public Vector3 Difference { get; set; }
    [Networked] public Vector3 Position { get; set; }
    [Networked] public float Rotation { get; set; }
    [Networked] public Vector3 OriginalPosition { get; set; }
    [Networked] public float OriginalRotation { get; set; }
    [Networked] public TileMask MaskTextureIndex { get; set; } = TileMask.C;
    [Networked] public int MainTextureIndex { get; set; } = -1;
    [Networked] public Vector4 BaseMapModifiers { get; set; }
    [Networked] public Vector4 MaskModifiers { get; set; }


    private MeshRenderer _renderer;

    private ChangeDetector _changeDetector;

    [SerializeField] private bool _debugDraw;

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector( ChangeDetector.Source.SimulationState );
        _renderer = GetComponentInChildren<MeshRenderer>();

        RefreshState();
    }

    public override void FixedUpdateNetwork()
    {
        if ( !HasInputAuthority && IsBeingDragged )
        {
            //Debug.Log( $"asdf {PlayerInputPoll.PlayerInputData.MousePosition}" );

            var screenPos = PlayerInputPoll.PlayerInputData.MousePosition;// Input.mousePosition;
            screenPos.z = -Camera.transform.position.z;
            var pos = Camera.ScreenToWorldPoint( screenPos ) - Difference;
            pos.z = -1f;

            transform.position = pos;
            
            //transform.SetPositionAndRotation( Position, Rotation );
        }
    }

    public void Drop()
    {
        var pos = transform.position;
        pos.z = -0.1f;
        transform.position = pos;
    }

    public void SendToOrinalPosition()
    {
        transform.position = OriginalPosition;
    }

    public override void Render()
    {
        var pBlock = new MaterialPropertyBlock();
        foreach ( var change in _changeDetector.DetectChanges( this ) )
        {
            switch ( change )
            {
                case nameof( MainTextureIndex ):
                    var baseMap = TextureManager.Instance.GetTexture( MainTextureIndex );
                    pBlock.SetTexture( "_BaseMap", baseMap );
                    _renderer.SetPropertyBlock( pBlock );
                    break;
                case nameof( MaskTextureIndex ):
                    var maskMap = TextureManager.Instance.GetMask( MaskTextureIndex );

                    pBlock.SetTexture( "_Mask", maskMap );
                    _renderer.SetPropertyBlock( pBlock );
                    break;
                case nameof( BaseMapModifiers ):
                    pBlock.SetVector( "_BaseMap_ST", BaseMapModifiers );
                    _renderer.SetPropertyBlock( pBlock );
                    break;
                case nameof( MaskModifiers ):
                    pBlock.SetVector( "_Mask_ST", MaskModifiers );
                    _renderer.SetPropertyBlock( pBlock );
                    break;
                case nameof( Position ):
                    transform.position = Position;
                    break;
                default:
                    break;
            }
        }
    }

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
        if ( !_debugDraw ) return;

        Gizmos.DrawSphere( OriginalPosition, 0.1f );

        if ( Vector3.Distance( transform.position, OriginalPosition ) <= 1 )
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawLine( transform.position, OriginalPosition );
    }
}
