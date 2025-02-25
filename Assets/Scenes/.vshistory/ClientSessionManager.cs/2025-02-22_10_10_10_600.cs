using Unity.Netcode;
using UnityEngine.Events;

public class ClientSessionManager : NetworkBehaviour
{
    public UnityEvent OnGameStart;


    [Rpc( SendTo.Everyone )]
    public void StartGameRpc()
    {
        OnGameStart?.Invoke();
    }

    //private void Update()
    //{
    //    if ( Input.GetKeyDown( KeyCode.F9 ) )
    //        _playersCount++;
    //    if ( Input.GetKeyDown( KeyCode.F10 ) )
    //        OnScore( PlayerScore.Right );
    //}

    //[Rpc( SendTo.Authority )]
    //private void ScoreboardCallRpc()
    //{
    //    ScoreboardUpdateRpc();
    //}

    //[Rpc( SendTo.Me )]
    //private void ScoreboardUpdateRpc()
    //{
    //    _rightPlayerScore.text = _rightScore.Value.ToString();
    //    _leftPlayerScore.text = _leftScore.Value.ToString();
    //}
}
