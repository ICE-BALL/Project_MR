using ServerCore;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static Connector _connector = new Connector();
    public static ServerSession _session = new ServerSession();

    public static Dictionary<int, Player> _players = new Dictionary<int, Player>();

    void Update()
    {
        if (PlayerManager.Instance._recvEvents.Count == PlayerManager.Instance._recvEventBuffers.Count && PlayerManager.Instance._recvEvents.Count > 0 && PlayerManager.Instance._recvEventBuffers.Count > 0)
        {
            for (int i = 0; i < PlayerManager.Instance._recvEvents.Count; i++)
            {
                IPacket p = PlayerManager.Instance._recvEventBuffers.Dequeue();
                PlayerManager.Instance._recvEvents.Dequeue().Invoke(p);
            }
        }
    }

    public static void Loading(string path)
    {
        IPAddress IPAddr = IPAddress.Parse(path);
        IPEndPoint endPoint = new IPEndPoint(IPAddr, 7777);

        _connector.Init(endPoint, () => _session);
    }
}
