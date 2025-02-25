using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class PlayerFactory
{
    public static GameObject CreatePlayer( GameObject prefab, Vector3 spawnPoint, ulong ownerId, bool isAI = false )
    {
        var playerInstance = GameObject.Instantiate( prefab, spawnPoint, Quaternion.identity );
        
        var netObj = playerInstance.GetComponent<NetworkObject>();

        if ( isAI )
        {
            GameObject.Destroy( playerInstance.GetComponent<PlayerController>() );
            playerInstance.AddComponent<NavMeshAgent>();
            playerInstance.AddComponent<AIPlayerController>();
            netObj.Spawn();
        }
        else
            netObj.SpawnWithOwnership( ownerId );
        
        return playerInstance;
    }
}
