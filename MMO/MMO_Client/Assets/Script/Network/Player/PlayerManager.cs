using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{
    static PlayerManager instance = new PlayerManager();
    public static PlayerManager Instance { get { return instance; } }

    public Queue<Action<IPacket>> _recvEvents = new Queue<Action<IPacket>>();
    public Queue<IPacket> _recvEventBuffers = new Queue<IPacket>();

    public List<GameObject> _players = new List<GameObject>();

}
