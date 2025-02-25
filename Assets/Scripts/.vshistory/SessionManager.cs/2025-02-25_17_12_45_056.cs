using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.Events;

public class SessionManager : NetworkBehaviour
{
    public static SessionManager Instance;

    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _ballHeight;
    [SerializeField] private Transform _leftSideSpawnPoint;
    [SerializeField] private Transform _rightSideSpawnPoint;
    [SerializeField] private Transform _courtDivisor;

    private GameObject _ball;

    public UnityEvent OnGameStart;

    private bool _isBallSpawn;

    private void Awake()
    {
        if ( Instance == null )
        {
            Instance = this;
        }
        else
        {
            Destroy( this );
        }
    }

    private async void Start()
    {
        try
        {
            await UnityServices.Instance.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            Debug.Log( $"User authenticated  {AuthenticationService.Instance.PlayerId}" );
        }
        catch
        {

        }
    }

    [Rpc( SendTo.Server )]
    public void ResetBallPositionRpc()
    {
        if ( !_isBallSpawn )
        {
            SpawnBall();
        }
        else
        {
            var rb = _ball.GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = false;
            _ball.transform.position = RandomBallPosition();
        }
    }

    public UniTask StartSession( string sessionId )
    {
        var playersLimit = PlayerPrefs.GetInt( PlayerPrefsKeys.RoomPlayersQuantity.ToString() );

        var options = new SessionOptions
        {
            Name = sessionId,
            MaxPlayers = playersLimit,
            IsPrivate = false
        }.WithRelayNetwork();

        return MultiplayerService.Instance.CreateOrJoinSessionAsync( sessionId, options ).AsUniTask();
    }

    [Rpc( SendTo.ClientsAndHost )]
    public void StartGameRpc()
    {
        OnGameStart?.Invoke();
        if ( IsServer )
        {
            SpawnPlayers();
        }
    }

    private void SpawnPlayers()
    {
        foreach ( var client in NetworkManager.Singleton.ConnectedClientsList )
        {
            var side = client.ClientId % 2 == 0 ? Side.Left : Side.Right;

            var spawnPoint = client.ClientId % 2 == 0 ? _leftSideSpawnPoint : _rightSideSpawnPoint;

            var playerInstance = PlayerFactory.CreatePlayer( _player, spawnPoint.position, client.ClientId );

            switch ( side )
            {
                case Side.Left:
                    playerInstance.layer = LayerMask.NameToLayer( "Player1" );
                    break;
                case Side.Right:
                    playerInstance.layer = LayerMask.NameToLayer( "Player2" );
                    break;
            }
        }

        if ( NetworkManager.Singleton.ConnectedClientsList.Count == 1 )
        {
            var spawnPoint = _rightSideSpawnPoint;

            var playerInstance = PlayerFactory.CreatePlayer( _player, spawnPoint.position, NetworkManager.Singleton.LocalClientId, true );

            playerInstance.layer = LayerMask.NameToLayer( "Player2" );
        }
    }

    private void SpawnBall()
    {
        if ( IsServer )
        {
            var position = RandomBallPosition();

            _ball = Instantiate( _ballPrefab );
            _ball.transform.position = position;

            var netObj = _ball.GetComponent<NetworkObject>();
            netObj.Spawn();
            _isBallSpawn = true;

            var aiPlayer = FindFirstObjectByType<AIPlayerController>();

            if ( aiPlayer != null )
            {
                var ball = _ball.GetComponent<BallBehaviour>();
                aiPlayer.Ball = ball;
                ball.CourtDivisor = _courtDivisor;
            }
        }
    }

    private Vector3 RandomBallPosition()
    {
        var randomPlace = Random.insideUnitCircle * 30;
        return ( ( _leftSideSpawnPoint.position + _rightSideSpawnPoint.position ) / 2 ) + new Vector3( randomPlace.x, _ballHeight, randomPlace.y );
    }

    [Rpc(SendTo.Server)]
    internal void HideBallRpc()
    {
        if ( _isBallSpawn )
        {
            var rb = _ball.GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            _ball.transform.position = Vector3.up * 1000;
            rb.isKinematic = true;
        }
    }
}