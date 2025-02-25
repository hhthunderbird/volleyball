using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.Android.Gradle;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class SessionManager : NetworkBehaviour
{
    public static SessionManager Instance;

    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _ballHeight;
    [SerializeField] private Transform _leftSideSpawnPoint;
    [SerializeField] private Transform _rightSideSpawnPoint;

    private List<BallTrajectory> _players = new();

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
            var side = client.ClientId % 2 == 0 ? PlayerSide.Left : PlayerSide.Right;

            var spawnPoint = client.ClientId % 2 == 0 ? _leftSideSpawnPoint : _rightSideSpawnPoint;

            var playerInstance = PlayerFactory.CreatePlayer( _player, spawnPoint.position, client.ClientId);

            _players.Add( playerInstance.GetComponent<BallTrajectory>() );

            switch ( side )
            {
                case PlayerSide.Left:
                    playerInstance.layer = LayerMask.NameToLayer( "Player1" );
                    break;
                case PlayerSide.Right:
                    playerInstance.layer = LayerMask.NameToLayer( "Player2" );
                    break;
            }

            var netObj = playerInstance.GetComponent<NetworkObject>();

            netObj.SpawnWithOwnership( client.ClientId );
        }

        if( NetworkManager.Singleton.ConnectedClientsList.Count == 1 )
        {
            var spawnPoint = _rightSideSpawnPoint;

            var playerInstance = PlayerFactory.CreatePlayer( _player, spawnPoint.position, NetworkManager.Singleton.LocalClientId, true);

            Destroy( playerInstance.GetComponent<PlayerController>() );
            playerInstance.AddComponent<NavMeshAgent>();
            playerInstance.AddComponent<AIPlayerController>();

            _players.Add( playerInstance.GetComponent<BallTrajectory>() );

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

            foreach ( var player in _players )
            {
                player.Ball = _ball;
            }
        }
    }

    private Vector3 RandomBallPosition()
    {
        var randomPlace = Random.insideUnitCircle * 30;
        return ( ( _leftSideSpawnPoint.position + _rightSideSpawnPoint.position ) / 2 ) + new Vector3( randomPlace.x, _ballHeight, randomPlace.y );
    }

}