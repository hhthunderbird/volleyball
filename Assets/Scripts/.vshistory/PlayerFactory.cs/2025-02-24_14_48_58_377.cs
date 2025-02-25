using Unity.Netcode;
using UnityEngine;

public class PlayerFactory
{
    public static GameObject CreatePlayer( GameObject prefab, Transform spawnPoint, ulong ownerId, bool isAI )
    {
        var playerInstance = GameObject.Instantiate( prefab, spawnPoint.position, Quaternion.identity );
        var player = playerInstance.GetComponent<PlayerController>();
        player.IsAiControlled = isAI;

        var netObj = playerInstance.GetComponent<NetworkObject>();
        netObj.SpawnWithOwnership( ownerId );

        return playerInstance;
    }
}
