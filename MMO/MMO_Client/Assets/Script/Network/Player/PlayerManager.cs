using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    static PlayerManager instance = new PlayerManager();
    public static PlayerManager Instance { get { return instance; } }

    public ConcurrentQueue<Action<IPacket>> _recvEvents = new ConcurrentQueue<Action<IPacket>>();
    public ConcurrentQueue<IPacket> _recvEventBuffers = new ConcurrentQueue<IPacket>();

    public List<GameObject> _players = new List<GameObject>();

}
