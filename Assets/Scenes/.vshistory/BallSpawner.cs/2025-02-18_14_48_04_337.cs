using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private float _ballHeight;


    
    void Start()
    {
        var randomPlace = Random.insideUnitCircle * 5;
        var position = new Vector3( randomPlace.x, _ballHeight, randomPlace.y );
        
    }
}
