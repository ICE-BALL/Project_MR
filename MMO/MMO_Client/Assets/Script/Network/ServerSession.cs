using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

public class ServerSession : PacketSession
{
    public override void OnConnect(EndPoint endPoint)
    {
        Debug.Log($"Connected to {endPoint}");
    }

    public override void OnDisconnect(EndPoint endPoint)
    {
        Debug.Log($"Disconnected {endPoint}");
    }

    public override void OnRecvPacket(ArraySegment<byte> Buffer)
    {
        ushort count = 0;
        count += sizeof(ushort);
        ushort Id = BitConverter.ToUInt16(Buffer.Array, Buffer.Offset + count);
        count += sizeof(ushort);
        switch (Id)
        {
            case (ushort)PacketID.PlayerList:
                PlayerList list = new PlayerList();
                list.Read(Buffer);
                PlayerManager.Instance._recvEvents.Enqueue(RecvPacket.Instance.Add);
                PlayerManager.Instance._recvEventBuffers.Enqueue(list);
                break;
            case (ushort)PacketID.BroadCastEnter:
                BroadCastEnter enter = new BroadCastEnter();
                enter.Read(Buffer);
                PlayerManager.Instance._recvEvents.Enqueue(RecvPacket.Instance.AddOther);
                PlayerManager.Instance._recvEventBuffers.Enqueue(enter);
                break;
            case (ushort)PacketID.BroadCastLeave:
                BroadCastLeave leave = new BroadCastLeave();
                leave.Read(Buffer);
                PlayerManager.Instance._recvEvents.Enqueue(RecvPacket.Instance.Leave);
                PlayerManager.Instance._recvEventBuffers.Enqueue(leave);
                break;
            case (ushort)PacketID.PlayerMove:
                PlayerMove pm = new PlayerMove();
                pm.Read(Buffer);
                PlayerManager.Instance._recvEvents.Enqueue(RecvPacket.Instance.MoveOtherPlayer);
                PlayerManager.Instance._recvEventBuffers.Enqueue(pm);
                break;
            case (ushort)PacketID.Data:
                Data data = new Data();
                data.Read(Buffer);
                PlayerManager.Instance._recvEvents.Enqueue(RecvPacket.Instance.SetData);
                PlayerManager.Instance._recvEventBuffers.Enqueue(data);
                break;
        }
    }

    public override void OnSend(int numOfbytes)
    {
        //Debug.Log($"Sended {numOfbytes} bytes");
    }
}
