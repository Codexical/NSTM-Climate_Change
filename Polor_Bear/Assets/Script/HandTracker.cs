// DepthStreamReceiver.cs
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net.Sockets;
using System.Threading;

public class HandTracker : MonoBehaviour
{

    [Header("Network Settings")]
    public string serverAddress = "127.0.0.1";
    public int serverPort = 8888;

    [Header("Image Settings")]
    public int imageWidth = 640;
    public int imageHeight = 360;

    [Header("UI Display")]
    public RawImage displayImage;

    [Header("Depth Filtering")]
    public int lowerBound;
    public int upperBound;

    private TcpClient client;
    private NetworkStream stream;
    private Thread clientThread;
    private bool isRunning = false;

    private Texture2D texture;
    private byte[] receivedBytes;
    private ushort[] depthDataBuffer;
    private ushort[] depthDataBufferNow;
    private ushort[] depthDataBufferBase;

    private readonly object frameLock = new object();
    private bool newFrameReady = false;

    void Start()
    {
        if (displayImage == null)
        {
            Debug.LogError("Display Image is not assigned.");
            return;
        }
        texture = new Texture2D(imageWidth, imageHeight, TextureFormat.R16, false);

        displayImage.texture = texture;

        receivedBytes = new byte[imageWidth * imageHeight * 2];
        depthDataBuffer = new ushort[imageWidth * imageHeight];
        depthDataBufferBase = new ushort[imageWidth * imageHeight];

        StartClientThread();
    }

    void Update()
    {
        if (newFrameReady)
        {
            lock (frameLock)
            {
                // Update the texture with new data and reset the flag
                texture.LoadRawTextureData(receivedBytes);
                texture.Apply();
                newFrameReady = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (displayImage.enabled)
            {
                displayImage.enabled = false;
            }
            else
            {
                displayImage.enabled = true;
            }
        }
    }

    void OnApplicationQuit()
    {
        StopClientThread();
    }

    void OnDestroy()
    {
        StopClientThread();
    }

    public bool[,] GetClickArea(bool isInit = false)
    {
        if (depthDataBuffer == null || !isRunning)
        {
            return null;
        }

        bool[,] depthArray2D = new bool[imageHeight, imageWidth];

        lock (frameLock)
        {
            // This operation is now safe because we are using a locked buffer
            for (int y = 0; y < imageHeight; y++)
            {
                for (int x = 0; x < imageWidth; x++)
                {
                    var depth = depthDataBuffer[y * imageWidth + x];
                    var diff = depthDataBufferBase[y * imageWidth + x] - depth;
                    depthArray2D[y, x] = diff > lowerBound && diff < upperBound;
                }
            }
            if (isInit)
            {
                for (int y = 0; y < imageHeight; y++)
                {
                    for (int x = 0; x < imageWidth; x++)
                    {
                        var depth = depthDataBuffer[y * imageWidth + x];
                        depthDataBufferBase[y * imageWidth + x] = (ushort)((float)depthDataBufferBase[y * imageWidth + x] * 0.99f + (float)depth * 0.01f);
                    }
                }
            }
        }

        return depthArray2D;
    }

    private void StartClientThread()
    {
        isRunning = true;
        clientThread = new Thread(new ThreadStart(NetworkLoop));
        clientThread.IsBackground = true;
        clientThread.Start();
    }

    private void StopClientThread()
    {
        isRunning = false; // Signal the thread to stop

        // Close connections immediately
        if (stream != null) stream.Close();
        if (client != null) client.Close();

        // Wait for the thread to finish its current operation and exit
        if (clientThread != null && clientThread.IsAlive)
        {
            clientThread.Join();
        }
        Debug.Log("Client thread stopped.");
    }

    private void NetworkLoop()
    {
        // This outer loop handles reconnection attempts
        while (isRunning)
        {
            try
            {
                Debug.Log($"Attempting to connect to {serverAddress}:{serverPort}...");
                client = new TcpClient(serverAddress, serverPort);
                stream = client.GetStream();
                Debug.Log("Connection successful!");

                byte[] messageSizeBytes = new byte[8];

                // This inner loop reads data while the connection is active
                while (isRunning)
                {
                    int bytesRead = ReadAll(stream, messageSizeBytes, 8);
                    if (bytesRead < 8)
                    {
                        Debug.LogWarning("Server disconnected. Breaking read loop.");
                        break; // Connection lost
                    }

                    // Use ToUInt64 to match Python's 'Q' format (unsigned 64-bit)
                    ulong messageSize = BitConverter.ToUInt64(messageSizeBytes, 0);

                    // Ensure the message size is reasonable before allocating memory
                    if (messageSize > 0 && messageSize < 10_000_000) // e.g., < 10MB
                    {
                        byte[] frameData = new byte[messageSize];
                        bytesRead = ReadAll(stream, frameData, (int)messageSize);
                        if (bytesRead < (int)messageSize)
                        {
                            Debug.LogWarning("Incomplete frame received. Breaking read loop.");
                            break; // Connection lost during frame transmission
                        }

                        lock (frameLock)
                        {
                            Buffer.BlockCopy(frameData, 0, receivedBytes, 0, frameData.Length);
                            Buffer.BlockCopy(frameData, 0, depthDataBuffer, 0, frameData.Length);
                            newFrameReady = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Log any errors that occur during connection or streaming
                Debug.LogWarning($"Network error: {e.Message}");
            }
            finally
            {
                // Clean up resources before the next connection attempt
                if (stream != null) stream.Close();
                if (client != null) client.Close();
            }

            // If the script is still supposed to be running, wait 1 second before retrying
            if (isRunning)
            {
                Debug.Log("Retrying connection in 1 second...");
                Thread.Sleep(1000);
            }
        }
    }

    private int ReadAll(NetworkStream stream, byte[] buffer, int size)
    {
        int totalBytesRead = 0;
        int bytesLeft = size;
        while (bytesLeft > 0)
        {
            int bytesRead = stream.Read(buffer, totalBytesRead, bytesLeft);
            if (bytesRead == 0)
            {
                // The connection has been closed by the remote host
                break;
            }
            totalBytesRead += bytesRead;
            bytesLeft -= bytesRead;
        }
        return totalBytesRead;
    }
}

