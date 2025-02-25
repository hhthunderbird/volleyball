using Fusion;
using UnityEngine;

public class Piece : NetworkBehaviour
{
    [Networked] public PlayerRef Owner { get; set; }

    //[SerializeField] private Vector2Int _index;
    public Vector3 OriginalLocation;// { get; set; }
    public float OriginalRotation;

    private Renderer _renderer;

    public Renderer Renderer
    {
        get
        {
            if ( _renderer == null )
                _renderer = GetComponent<Renderer>();
            return _renderer;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere( OriginalLocation, 0.1f );
        Gizmos.DrawLine( transform.position, OriginalLocation );
    }
}
