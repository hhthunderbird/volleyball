using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreBoard : NetworkBehaviour
{
    [SerializeField] private BallBehaviour _ballBehaviour;
    [SerializeField] private TMP_Text _leftPlayerScore;
    [SerializeField] private TMP_Text _rightPlayerScore;

    private NetworkVariable<int> _leftScore = new();
    private NetworkVariable<int> _rightScore = new();


    private void Start()
    {
        _leftScore.OnValueChanged += UpdateScore;
        _rightScore.OnValueChanged += UpdateScore;
    }

    private void UpdateScore( int previousValue, int newValue )
    {
        throw new NotImplementedException();
    }

    [Rpc(SendTo.Server)]
    public void OnBallFallRpc( CourtSide side )
    {
        switch ( side )
        {
            case CourtSide.Left:
                OnScore( PlayerScore.Right );
                break;
            case CourtSide.Right:
                OnScore( PlayerScore.Left );
                break;
            default:
                break;
        }
    }

    private void OnScore( PlayerScore score )
    {
        switch ( score )
        {
            case PlayerScore.Left:
                _leftScore.Value++;
                break;
            case PlayerScore.Right:
                _rightScore.Value++;
                break;
        }
        ScoreboardCallRpc();
    }

    private void ScoreboardCallRpc()
    {
        _rightPlayerScore.text = _rightScore.Value.ToString();
        _leftPlayerScore.text = _leftScore.Value.ToString();
        
    }

}
