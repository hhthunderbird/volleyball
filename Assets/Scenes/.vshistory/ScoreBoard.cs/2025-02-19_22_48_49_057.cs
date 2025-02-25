using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreBoard : NetworkBehaviour
{
    [SerializeField] private BallBehaviour _ballBehaviour;
    [SerializeField] private TMP_Text _leftPlayerScore;
    [SerializeField] private TMP_Text _rightPlayerScore;


    private NetworkVariable<int> _leftScore = new ();
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

    private void OnValueChangedLeft( int previousValue, int newValue )
    {
        throw new NotImplementedException();
    }

    private void OnScore( PlayerScore score )
    {
        switch ( score )
        {
            case PlayerScore.Left:
                _leftScore.Value++;
                _leftPlayerScore.text = _leftScore.Value.ToString();
                break;
            case PlayerScore.Right:
                _rightScore.Value++;
                _rightPlayerScore.text = _rightScore.Value.ToString();
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
