using UnityEngine;

public class Piece : MonoBehaviour
{
    //[SerializeField] private Vector2Int _index;
    public Vector3 OriginalLocation;// { get; set; }

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

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
