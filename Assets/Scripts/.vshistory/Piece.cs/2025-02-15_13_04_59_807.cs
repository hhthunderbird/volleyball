using Unity.Netcode;
using UnityEngine;


public class Piece : NetworkBehaviour
{
    private NetworkVariable<Vector3> _originalPosition = new( default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner );
    public Vector3 OriginalPosition
    {
        get => _originalPosition.Value;
        set => _originalPosition.Value = value;
    }

    private NetworkVariable<float> _originalRotation = new( default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner );
    public float OriginalRotation
    {
        get => _originalRotation.Value;
        set => _originalRotation.Value = value;
    }

    private NetworkVariable<int> _textureId = new( default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner );
    public int TextureId
    {
        get => _textureId.Value;
        set => _textureId.Value = value;
    }

    private NetworkVariable<Vector4> _baseMapModifiers = new( default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner );
    public Vector4 BaseMapModifiers
    {
        get => _baseMapModifiers.Value;
        set => _baseMapModifiers.Value = value;
    }
    //public Vector4 MaskModifiers { get; set; }


    private MeshRenderer _renderer;

    [SerializeField] private bool _debugDraw;

    public override void OnNetworkSpawn()
    {
        if ( !IsOwner )
        {
            _renderer = GetComponentInChildren<MeshRenderer>();

            RefreshState();
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

    private void RefreshState()
    {
        //Debug.LogWarning( "aaaaaaaaa" );
        var pBlock = new MaterialPropertyBlock();

        var baseMap = TextureManager.Instance.GetTexture( _textureId.Value );
        //var maskMap = TextureManager.Instance.GetMask( MaskTextureIndex );

        pBlock.SetTexture( "_BaseMap", baseMap );

        //pBlock.SetTexture( "_Mask", maskMap );

        pBlock.SetVector( "_BaseMap_ST", BaseMapModifiers );

        //pBlock.SetVector( "_Mask_ST", MaskModifiers );

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
