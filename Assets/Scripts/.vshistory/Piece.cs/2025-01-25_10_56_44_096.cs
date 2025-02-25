using Fusion;
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

    public override void Spawned()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void BasemapChanged()
    {

    }
    private void MaskChanged()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere( OriginalLocation, 0.1f );
        Gizmos.DrawLine( transform.position, OriginalLocation );
    }
}
