using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField] private float _kickStrength;

    private void OnCollisionEnter( Collision collision )
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player1"))

        if(collision.gameObject.layer == LayerMask.NameToLayer("Player2"))
    }
}
