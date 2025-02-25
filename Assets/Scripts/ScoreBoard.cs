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

    public bool DoReset => _leftScore.Value == 7 || _rightScore.Value == 7;

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
    public void ResetScoreRpc()
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
                OnScore( Side.Right );
                break;
            case CourtSide.Right:
                OnScore( Side.Left );
                break;
            default:
                break;
        }
    }

    private void OnScore( Side score )
    {
        switch ( score )
        {
            case Side.Left:
                _leftScore.Value++;
                break;
            case Side.Right:
                _rightScore.Value++;
                break;
        }
    }
}
