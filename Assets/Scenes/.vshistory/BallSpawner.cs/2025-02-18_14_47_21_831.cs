using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private float _ballHeight;


    
    void Start()
    {
        Vector2 randomPlace = Random.insideUnitCircle();
        
    }
}
