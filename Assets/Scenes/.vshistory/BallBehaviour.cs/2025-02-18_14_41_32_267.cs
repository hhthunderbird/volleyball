using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField] private float _kickStrength;
    private Rigidbody _ballRB;

    private void Start()
    {
        _ballRB = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter( Collision collision )
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player1"))

        if(collision.gameObject.layer == LayerMask.NameToLayer("Player2"))
    }
}
