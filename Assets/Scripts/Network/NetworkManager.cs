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

            //Add Ondestroy Event
            int connectionId = _fromClient; //local copy to use current _fromClient as value inside lambda function
            MyServer.clients[_fromClient].player.GetComponent<PlayerPoint>().OnDestroyEvent += () =>
            {
                if(MyServer.clients[connectionId].SSI != null)
                {
                    StopCoroutine(MyServer.clients[connectionId].SSI);
                }

                float fitness = MyServer.clients[connectionId].player.GetComponent<PlayerFitness>().fitness;

                Destroy(MyServer.clients[connectionId].cameraHolder);
                Destroy(MyServer.clients[connectionId].player);

                List<byte> _packet = new List<byte>();
                _packet.AddRange(BitConverter.GetBytes((ushort)0x3000));
                _packet.AddRange(BitConverter.GetBytes(fitness));
                server.Send(connectionId, new ArraySegment<byte>(_packet.ToArray()));
            };

            //Send captured Image(coroutine which waits until image is encoded)
            MyServer.clients[_fromClient].SSI = StartCoroutine(StartSendImage(_fromClient));

        });
        MyServer.packetHandlers.Add(0x2000, (int _fromClient, ArraySegment<byte> _packet) =>
        {
            float output;
            Debug.Log($"received 0x2000");

            if (MyServer.clients[_fromClient].player == null)
            {
                Debug.Log("Failed to handle 0x2000 packet because there was no player object");
                return;
            }

            //update _fromClient player movement
            output = BitConverter.ToSingle(_packet.Array, 2 + _packet.Offset + 4 * 0);
            Debug.Log($"forwardForce: {output}");
            MyServer.clients[_fromClient].player.GetComponent<PlayerMovement>().forwardForce = output;
            output = BitConverter.ToSingle(_packet.Array, 2 + _packet.Offset + 4 * 1);
            Debug.Log($"horizontalForce: {output}");
            MyServer.clients[_fromClient].player.GetComponent<PlayerMovement>().horizontalForce = output;
            output = BitConverter.ToSingle(_packet.Array, 2 + _packet.Offset + 4 * 2);
            Debug.Log($"xRotationSpeed: {output}");
            MyServer.clients[_fromClient].player.GetComponent<PlayerMovement>().xRotationSpeed = output;
            output = BitConverter.ToSingle(_packet.Array, 2 + _packet.Offset + 4 * 3);
            Debug.Log($"yRotationSpeed: {output}");
            MyServer.clients[_fromClient].player.GetComponent<PlayerMovement>().yRotationSpeed = output;

            MyServer.clients[_fromClient].SSI = StartCoroutine(StartSendImage(_fromClient));
        });
        MyServer.packetHandlers.Add(0x3000, (int _fromClient, ArraySegment<byte> _packet) =>
        {
            int generation = BitConverter.ToInt32(_packet.Array, 2+_packet.Offset);
            Debug.Log($"received 0x3000. Generation: {generation}");

            //Instantiate Player and Camera Prefab
            MyServer.clients[_fromClient].CreatePlayer();
            int connectionId = _fromClient; //local copy to use current _fromClient as value inside lambda function
            MyServer.clients[_fromClient].player.GetComponent<PlayerPoint>().OnDestroyEvent += () =>
            {
                if (MyServer.clients[connectionId].SSI != null)
                {
                    StopCoroutine(MyServer.clients[connectionId].SSI);
                }

                float fitness = MyServer.clients[connectionId].player.GetComponent<PlayerFitness>().fitness;

                Destroy(MyServer.clients[connectionId].cameraHolder);
                Destroy(MyServer.clients[connectionId].player);

                List<byte> _packet = new List<byte>();
                _packet.AddRange(BitConverter.GetBytes((ushort)0x3000));
                _packet.AddRange(BitConverter.GetBytes(fitness));
                server.Send(connectionId, new ArraySegment<byte>(_packet.ToArray()));
            };
            //Send captured Image(coroutine which waits until image is encoded)
            MyServer.clients[_fromClient].SSI = StartCoroutine(StartSendImage(_fromClient));
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
        CameraCapture cameraCapture = MyServer.clients[connectionId].cameraHolder.transform.Find("PlayerCamera").GetComponent<CameraCapture>();
        cameraCapture.StartCaptureAndSaveImage();

        yield return new WaitUntil(() => cameraCapture.CompleteCaptureRequest); //카메라 출력 JPEG 변환이 완료될때까지 코루틴 실행 시점 연장

        cameraCapture.CompleteCaptureRequest = false;
        List<byte> _packet = new List<byte>();
        _packet.AddRange(BitConverter.GetBytes((ushort)0x2000));
        _packet.AddRange(BitConverter.GetBytes(MyServer.clients[connectionId].player.GetComponent<PlayerPoint>().getdReward()));
        _packet.AddRange(BitConverter.GetBytes(cameraCapture.bytes.Length));
        _packet.AddRange(cameraCapture.bytes);
        server.Send(connectionId, new ArraySegment<byte>(_packet.ToArray()));
    }
}
