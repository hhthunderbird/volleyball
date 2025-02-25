using System;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private BallBehaviour _ballBehaviour;
    void Start()
    {
        _ballBehaviour.OnScore += OnScore;
    }

    private void OnScore( PlayerScore score )
    {
        switch ( score )
        {
            case PlayerScore.Left:
                break;
            case PlayerScore.Right:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
