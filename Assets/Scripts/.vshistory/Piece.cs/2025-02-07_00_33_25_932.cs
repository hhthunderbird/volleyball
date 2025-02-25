using Unity.Netcode;
using UnityEngine;


public class Piece : NetworkBehaviour
{
    public Vector3 Position { get; set; }
    public float Rotation { get; set; }
    public Vector3 OriginalPosition { get; set; }
    public float OriginalRotation { get; set; }
    public TileMask MaskTextureIndex { get; set; } = TileMask.C;
    public int MainTextureIndex { get; set; } = -1;
    public Vector4 BaseMapModifiers { get; set; }
    public Vector4 MaskModifiers { get; set; }


    private MeshRenderer _renderer;

    [SerializeField] private bool _debugDraw;

    public override void OnNetworkSpawn()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();

        RefreshState();
    }

    //public override void FixedUpdateNetwork()
    //{
    //    if ( !HasInputAuthority && IsBeingDragged )
    //    {
    //        //Debug.Log( $"asdf {PlayerInputPoll.PlayerInputData.MousePosition}" );

    //        var screenPos = PlayerInputPoll.PlayerInputData.MousePosition;// Input.mousePosition;
    //        screenPos.z = -Camera.transform.position.z;
    //        var pos = Camera.ScreenToWorldPoint( screenPos ) - Difference;
    //        pos.z = -1f;

    //        transform.position = pos;

    //        //transform.SetPositionAndRotation( Position, Rotation );
    //    }
    //}

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
