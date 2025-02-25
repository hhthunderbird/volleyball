using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ImageTransfer : NetworkBehaviour
{
    private const int ChunkSize = 1024 * 32; // 32 KB per chunk
    private byte[] _receivedImageData;// = File.ReadAllBytes( imagePath );

    private List<byte[]> _receivedChunks = new List<byte[]>();
    private int totalChunks;

    public event Action<Texture2D> OnReceivedImage;


    public void SendImageToPeers( byte[] imageData )
    {
        int totalChunks = Mathf.CeilToInt( ( float ) imageData.Length / ChunkSize );

        for ( int i = 0; i < totalChunks; i++ )
        {
            int offset = i * ChunkSize;
            int length = Mathf.Min( ChunkSize, imageData.Length - offset );
            byte[] chunk = new byte[ length ];
            System.Array.Copy( imageData, offset, chunk, 0, length );

            // Send the chunk to all peers
            SendChunkToPeersRpc( chunk, i, totalChunks );
        }
    }

    [Rpc( SendTo.NotMe )]
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
            _receivedChunks.Clear();
            this.totalChunks = totalChunks;
        }

        _receivedChunks.Add( chunk );

        // Check if all chunks have been received
        if ( _receivedChunks.Count == totalChunks )
        {
            ReconstructImage();
        }
    }

    public Texture2D ReconstructImage()
    {
        // Combine all chunks into a single byte array
        byte[] imageData = new byte[ totalChunks * ChunkSize ];
        int offset = 0;

        foreach ( var chunk in _receivedChunks )
        {
            System.Buffer.BlockCopy( chunk, 0, imageData, offset, chunk.Length );
            offset += chunk.Length;
        }

        // Convert the byte array to a texture
        Texture2D texture = new Texture2D( 2, 2 );
        texture.LoadImage( imageData );
        return texture;
        // Apply the texture to the puzzle
        //PuzzleManager.Instance.CreatePuzzle( texture );
    }
}