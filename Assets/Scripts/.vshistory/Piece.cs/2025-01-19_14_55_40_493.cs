using UnityEngine;

public class Piece : MonoBehaviour
{
    //[SerializeField] private Vector2Int _index;
    public Vector3 OriginalLocation;// { get; set; }

    private SpriteRenderer _renderer;
    private SpriteMask _mask;
    public SpriteRenderer Renderer 
    {
        get
        {
            if ( _renderer == null )
                _renderer = GetComponent<SpriteRenderer>();
            return _renderer;
        }
    }

    public SpriteRenderer Mask
    {
        get
        {
            if ( _renderer == null )
                _renderer = GetComponent<SpriteRenderer>();
            return _renderer;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
