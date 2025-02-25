using Fusion;
using UnityEngine;

public class Piece : NetworkBehaviour
{
    [Networked] public PlayerRef Owner { get; set; }
    [Networked] public Vector3 OriginalLocation { get; set; }
    [Networked] public float OriginalRotation { get; set; }

    private ChangeDetector _changeDetector;

    private MeshRenderer _renderer;
    public MeshRenderer Renderer => _renderer;

    public override void Spawned()
    {
        _renderer = GetComponent<MeshRenderer>();
        _changeDetector = GetChangeDetector( ChangeDetector.Source.SimulationState );
    }

    public override void Render()
    {
        foreach ( var change in _changeDetector.DetectChanges(this) )
        {

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere( OriginalLocation, 0.1f );
        Gizmos.DrawLine( transform.position, OriginalLocation );
    }
}
