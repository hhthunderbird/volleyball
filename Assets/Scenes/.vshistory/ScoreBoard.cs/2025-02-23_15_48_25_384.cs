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

    public NetworkVariable<int> Score = new();

    private void Start()
    {
        _leftScore.OnValueChanged += UpdateScore;
        _rightScore.OnValueChanged += UpdateScore;
    }

    private void UpdateScore( int previousValue, int newValue )
    {
        if ( newValue >= 7 )
        {
            SessionManager.Instance.ResetBallPositionRpc();
            ResetScoreRpc();
            return;
        }

        _rightPlayerScore.text = _rightScore.Value.ToString();
        _leftPlayerScore.text = _leftScore.Value.ToString();
    }

    [Rpc(SendTo.Server)]
    private void ResetScoreRpc()
    {
        _leftScore.Value = 0;
        _rightScore.Value = 0;
    }


    [Rpc( SendTo.Server )]
    public void OnBallFallRpc( CourtSide side )
    {
        switch ( side )
        {
            case CourtSide.Left:
                OnScore( PlayerSide.Right );
                break;
            case CourtSide.Right:
                OnScore( PlayerSide.Left );
                break;
            default:
                break;
        }
    }

    private void OnScore( PlayerSide score )
    {
        switch ( score )
        {
            case PlayerSide.Left:
                _leftScore.Value++;
                break;
            case PlayerSide.Right:
                _rightScore.Value++;
                break;
        }
    }

}
