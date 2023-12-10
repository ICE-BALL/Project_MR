using ServerCore;
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
