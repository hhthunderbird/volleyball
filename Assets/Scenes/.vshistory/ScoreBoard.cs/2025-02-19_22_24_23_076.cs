using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreBoard : NetworkBehaviour
{
    [SerializeField] private BallBehaviour _ballBehaviour;
    [SerializeField] private TMP_Text _leftPlayerScore;
    [SerializeField] private TMP_Text _rightPlayerScore;
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

    
}
