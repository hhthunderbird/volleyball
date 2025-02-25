using UnityEngine;

public class Piece : MonoBehaviour
{
    //[SerializeField] private Vector2Int _index;
    public Vector3 OriginalLocation;// { get; set; }

    private SpriteRenderer _renderer;
    public SpriteRenderer Renderer => _renderer ?? GetComponent<SpriteRenderer>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
