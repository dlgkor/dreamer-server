using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyServer
{
    public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
    public delegate void PacketHandler(int _fromClient, ArraySegment<byte> _packet);
    public static Dictionary<ushort, PacketHandler> packetHandlers;

    static MyServer()
    {
        packetHandlers = new Dictionary<ushort, PacketHandler>();
        clients = new Dictionary<int, Client>();
    }
}
