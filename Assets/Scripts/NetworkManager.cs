using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    private Telepathy.Server server;
    public int port = 2005;

    public GameObject playerPrefab;
    public GameObject cameraHolderPrefab;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MyServer.packetHandlers.Add(0x1000, (int _fromClient, ArraySegment<byte> _packet) =>
        {
            int generation = BitConverter.ToInt32(_packet.Array, 2+_packet.Offset);
            Debug.Log($"received 0x1000. Generation: {generation}");

            //Instantiate Player and Camera Prefab
            MyServer.clients[_fromClient].CreatePlayer();
            //Send captured Image(coroutine which waits until image is encoded)
            StartCoroutine(StartSendImage(_fromClient));

        });
        MyServer.packetHandlers.Add(0x2000, (int _fromClient, ArraySegment<byte> _packet) =>
        {
            int outputSize = 4;
            Debug.Log($"received 0x2000");
            for (int i = 0; i < outputSize; i++)
            {
                double output = BitConverter.ToSingle(_packet.Array, 2+_packet.Offset + outputSize * 4);
                Debug.Log($"{output}");
                //update _fromClient player movement
            }


            if (MyServer.clients[_fromClient].player == null)
            {
                Debug.Log("Failed to handle 0x2000 packet because there was no player object");
                return;
            }
            //check if playerpoint is below 0
            if (MyServer.clients[_fromClient].player.GetComponent<PlayerPoint>().getPoint() <= 0)
            {
                //send fitness packet
                //###작성중###




                //destroy player and camera
                MyServer.clients[_fromClient].DestroyObjects();
            }
            else
            {
                //send captured Image(coroutine which waits until image is encoded)
                StartCoroutine(StartSendImage(_fromClient));
            }
        });
        MyServer.packetHandlers.Add(0x3000, (int _fromClient, ArraySegment<byte> _packet) =>
        {
            int generation = BitConverter.ToInt32(_packet.Array, 2+_packet.Offset);
            Debug.Log($"received 0x3000. Generation: {generation}");

            //Instantiate Player and Camera Prefab
            MyServer.clients[_fromClient].CreatePlayer();
            //Send captured Image(coroutine which waits until image is encoded)
            StartCoroutine(StartSendImage(_fromClient));
        });

        server = new Telepathy.Server(65536);
        server.OnConnected = OnClientConnected;
        server.OnData = OnClientdata;
        server.OnDisconnected = OnClientDisconnected;

        server.Start(port);
        Debug.Log($"Server started on port {port}");
    }

    // Update is called once per frame
    void Update()
    {
        server.Tick(100);
    }

    void OnClientConnected(int connectionId)
    {
        Debug.Log($"Client connected: {connectionId}");
        MyServer.clients.Add(connectionId, new Client(connectionId));

        List<byte> _packet = new List<byte>();
        _packet.AddRange(BitConverter.GetBytes((ushort)0x1000));
        server.Send(connectionId, new ArraySegment<byte>(_packet.ToArray()));
        Debug.Log($"send message {_packet.Count}");
    }

    void OnClientdata(int connectionId, ArraySegment<byte> data) {
        string message = System.Text.Encoding.UTF8.GetString(data);
        Debug.Log($"Received from {connectionId}: {data.Count}");
        MyServer.clients[connectionId].HandleData(data);
    }

    void OnClientDisconnected(int connectionId)
    {
        Debug.Log($"Client Disconnected: {connectionId}");
        MyServer.clients[connectionId].DestroyObjects();
        MyServer.clients.Remove(connectionId);
    }

    void OnApplicationQuit()
    {
        server.Stop();
    }

    public GameObject InstantiatePlayer()
    {
        return Instantiate(playerPrefab, new Vector3(0f, 3.0f, 0f), Quaternion.identity);
    }

    public void DestroyPlayer(GameObject player)
    {
        if (player == null)
            return;

        Destroy(player);
    }

    public GameObject InstantiateCameraHolder()
    {
        return Instantiate(cameraHolderPrefab, new Vector3(0f, 3.0f, 0f), Quaternion.identity);
    }

    public void DestroyCameraHolder(GameObject cameraHolder)
    {
        if (cameraHolder == null)
            return;

        Destroy(cameraHolder);
    }


    IEnumerator StartSendImage(int connectionId)
    {
        if (MyServer.clients[connectionId].cameraHolder == null)
        {
            Debug.Log("Failed to send image because there was no cameraHolder");
            yield break;
        }
        CameraCapture cameraCapture = MyServer.clients[connectionId].cameraHolder.transform.GetChild(0).GetComponent<CameraCapture>();
        cameraCapture.StartCaptureAndSaveImage();

        yield return new WaitUntil(() => cameraCapture.CompleteCaptureRequest); //카메라 출력 JPEG 변환이 완료될때까지 코루틴 실행 시점 연장

        cameraCapture.CompleteCaptureRequest = false;
        List<byte> _packet = new List<byte>();
        _packet.AddRange(BitConverter.GetBytes((ushort)0x2000));
        _packet.AddRange(cameraCapture.bytes);
        server.Send(connectionId, new ArraySegment<byte>(_packet.ToArray()));
    }
}
