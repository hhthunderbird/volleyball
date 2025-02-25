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

    void Start()
    {
    }

    public override void OnNetworkSpawn()
    {
        //_ballBehaviour.OnScore += OnScore;
        _leftScore.OnValueChanged += OnValueChangedLeft;
        _rightScore.OnValueChanged += OnValueChangedRight;
    }

    private void OnValueChangedRight( int previousValue, int newValue )
    {
        _rightPlayerScore.text = newValue.ToString();
    }

    private void OnValueChangedLeft( int previousValue, int newValue )
    {
        _leftPlayerScore.text = newValue.ToString();
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
    }

    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.F9 ) )
            OnScore( PlayerScore.Left );
        if ( Input.GetKeyDown( KeyCode.F10 ) )
            OnScore( PlayerScore.Right );
    }

}
