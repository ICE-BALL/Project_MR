using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PacketManager
{
    #region Singleton
    static PacketManager _instance = new PacketManager();
    public static PacketManager Instance { get { return _instance; } }
    #endregion

    PacketManager()
    {
        Register();
    }

    public void Register()
    {
      
        _handle.Add((ushort)PacketID.PlayerList, PacketHandler.PlayerListHandler);
        _onRecv.Add((ushort)PacketID.PlayerList, MakePacket<PlayerList>);
      
        _handle.Add((ushort)PacketID.BroadCastEnter, PacketHandler.BroadCastEnterHandler);
        _onRecv.Add((ushort)PacketID.BroadCastEnter, MakePacket<BroadCastEnter>);
      
        _handle.Add((ushort)PacketID.BroadCastLeave, PacketHandler.BroadCastLeaveHandler);
        _onRecv.Add((ushort)PacketID.BroadCastLeave, MakePacket<BroadCastLeave>);
      
        _handle.Add((ushort)PacketID.PlayerMove, PacketHandler.PlayerMoveHandler);
        _onRecv.Add((ushort)PacketID.PlayerMove, MakePacket<PlayerMove>);
      
        _handle.Add((ushort)PacketID.Data, PacketHandler.DataHandler);
        _onRecv.Add((ushort)PacketID.Data, MakePacket<Data>);
      
        _handle.Add((ushort)PacketID.MonsterList, PacketHandler.MonsterListHandler);
        _onRecv.Add((ushort)PacketID.MonsterList, MakePacket<MonsterList>);
      
        _handle.Add((ushort)PacketID.MonsterData, PacketHandler.MonsterDataHandler);
        _onRecv.Add((ushort)PacketID.MonsterData, MakePacket<MonsterData>);
      
        _handle.Add((ushort)PacketID.MonsterMove, PacketHandler.MonsterMoveHandler);
        _onRecv.Add((ushort)PacketID.MonsterMove, MakePacket<MonsterMove>);

    }

    Dictionary<ushort, Action<PacketSession, IPacket>> _handle = new Dictionary<ushort, Action<PacketSession, IPacket>>();
    Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>>();

    public void OnRecvPacket(PacketSession session, ArraySegment<byte> Buffer)
    {
        ushort count = 0;
        count += sizeof(ushort);
        ushort Id = BitConverter.ToUInt16(Buffer.Array, Buffer.Offset + count);
        Action<PacketSession, ArraySegment<byte>> action = null;
        if (_onRecv.TryGetValue(Id, out action))
            action.Invoke(session, Buffer);
    }

    void MakePacket<T>(PacketSession session, ArraySegment<byte> Buffer) where T : IPacket, new()
    {
        T pkt = new T();
        pkt.Read(Buffer);
        Action<PacketSession, IPacket> action = null;
        if (_handle.TryGetValue(pkt.Protocol, out action))
            action.Invoke(session, pkt);
    }
}
