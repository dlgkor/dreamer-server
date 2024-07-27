using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client
{
    private int clientId;
    public GameObject player;
    public GameObject cameraHolder;
    public Coroutine SSI;

    public Client(int clientId)
    {
        this.clientId = clientId;
    }
    public void HandleData(ArraySegment<byte> data)
    {
        ushort _packetID = BitConverter.ToUInt16(data.Array, data.Offset);

        MyServer.packetHandlers[_packetID](clientId, data);

        return;
    }

    public void CreatePlayer()
    {
        //Instantiate Player and CameraHolder
        player = NetworkManager.instance.InstantiatePlayer();
        cameraHolder = NetworkManager.instance.InstantiateCameraHolder();

        //Make cameraHolder to follow player object
        cameraHolder.GetComponent<FollowPlayer>().StartFollow(player.transform);
    }

    public void DestroyObjects()
    {
        //Destroy Player and CameraHolder gameObject
        NetworkManager.instance.DestroyCameraHolder(cameraHolder);
        NetworkManager.instance.DestroyPlayer(player);
    }
}
