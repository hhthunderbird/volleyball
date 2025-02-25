using Unity.Netcode;
using UnityEngine;

public class PlayerFactory
{
    public static GameObject CreatePlayer( GameObject prefab, Vector3 spawnPoint, ulong ownerId, bool isAI = false )
    {
        var playerInstance = GameObject.Instantiate( prefab, spawnPoint, Quaternion.identity );
        var player = playerInstance.GetComponent<PlayerController>();
        
        var netObj = playerInstance.GetComponent<NetworkObject>();

        if ( isAI )
            netObj.Spawn();
        else
            netObj.SpawnWithOwnership( ownerId );
        
        return playerInstance;
    }
}
