using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ImageTransfer : NetworkBehaviour
{
    private const int ChunkSize = 1024 * 32; // 32 KB per chunk
    private byte[] _imageData;// = File.ReadAllBytes( imagePath );

    private List<byte[]> receivedChunks = new List<byte[]>();
    private int totalChunks;

    [Rpc( SendTo.Server )]
    private void SendChunkToPeersRpc( byte[] chunk, int chunkIndex, int totalChunks, RpcParams rpcParams = default )
    {
        // Broadcast the chunk to all clients
        ReceiveChunkClientRpc( chunk, chunkIndex, totalChunks );
    }

    [ClientRpc]
    private void ReceiveChunkClientRpc( byte[] chunk, int chunkIndex, int totalChunks )
    {
        if ( IsOwner ) return; // Skip if this is the sender

        // Store the received chunk
        if ( chunkIndex == 0 )
        {
            receivedChunks.Clear();
            this.totalChunks = totalChunks;
        }

        receivedChunks.Add( chunk );

        // Check if all chunks have been received
        if ( receivedChunks.Count == totalChunks )
        {
            ReconstructImage();
        }
    }

    private void ReconstructImage()
    {
        // Combine all chunks into a single byte array
        byte[] imageData = new byte[ totalChunks * ChunkSize ];
        int offset = 0;

        foreach ( var chunk in receivedChunks )
        {
            System.Buffer.BlockCopy( chunk, 0, imageData, offset, chunk.Length );
            offset += chunk.Length;
        }

        // Convert the byte array to a texture
        Texture2D texture = new Texture2D( 2, 2 );
        texture.LoadImage( imageData );

        // Apply the texture to the puzzle
        PuzzleManager.Instance.CreatePuzzle( texture );
    }
}