using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketGenerator
{
    internal class PacketFormat
    {
        // {0} register packet
        public static string managerFormat =
@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PacketManager
{{
    #region Singleton
    static PacketManager _instance = new PacketManager();
    public static PacketManager Instance {{ get {{ return _instance; }} }}
    #endregion

    PacketManager()
    {{
        Register();
    }}

    public void Register()
    {{
{0}
    }}

    Dictionary<ushort, Action<PacketSession, IPacket>> _handle = new Dictionary<ushort, Action<PacketSession, IPacket>>();
    Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>>();

    public void OnRecvPacket(PacketSession session, ArraySegment<byte> Buffer)
    {{
        ushort count = 0;
        count += sizeof(ushort);
        ushort Id = BitConverter.ToUInt16(Buffer.Array, Buffer.Offset + count);
        Action<PacketSession, ArraySegment<byte>> action = null;
        if (_onRecv.TryGetValue(Id, out action))
            action.Invoke(session, Buffer);
    }}

    void MakePacket<T>(PacketSession session, ArraySegment<byte> Buffer) where T : IPacket, new()
    {{
        T pkt = new T();
        pkt.Read(Buffer);
        Action<PacketSession, IPacket> action = null;
        if (_handle.TryGetValue(pkt.Protocol, out action))
            action.Invoke(session, pkt);
    }}
}}
";

        // {0} packet name
        public static string managerRegisterFormat =
@"      
        _handle.Add((ushort)PacketID.{0}, PacketHandler.{0}Handler);
        _onRecv.Add((ushort)PacketID.{0}, MakePacket<{0}>);";

        // {0} Packet Id
        // {1} Packets
        public static string fileFormat =
@"using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public enum PacketID
{{
    {0}
}}

public enum PacketType
{{
    Send,
    BroadCastSend,
}}

public interface IPacket
{{
    ushort Protocol {{ get; }}
    void Read(ArraySegment<byte> segment);
    ArraySegment<byte> Write();
}}

{1}
";
        // {0} packet name
        // {1} packet number
        public static string packetEnumFormat =
@"{0} = {1},";

        // {0} packet name
        // {1} mamber var
        // {2} Read
        // {3} Write
        public static string packetFormat =
@"
public class {0} : IPacket
{{
    public ushort Protocol {{ get {{ return (ushort)PacketID.{0}; }} }}

    {1}

    public void Read(ArraySegment<byte> segment)
    {{
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        {2}
    }}

    public ArraySegment<byte> Write()
    {{
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        {3}

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }}
}}
";
        // {0} Type
        // {1} Name
        public static string memberFormat =
@"public {0} {1};
";

        // {0} List Name [capital letter]
        // {1} List Name [small letter]
        // {2} member
        // {3} Read
        // {4} Write
        public static string memberListFormat =
@"
public class {0}
{{
    {2}

    public void Read(ArraySegment<byte> segment, ref ushort count)
    {{
        {3}
    }}

    public void Write(ArraySegment<byte> segment, ref ushort count)
    {{
        {4}
    }}
}}
public List<{0}> {1}s = new List<{0}>();
";

        // {0} var Name
        // {1} To~
        // {2} Type
        public static string readFormat =
@"this.{0} = BitConverter.{1}(segment.Array, segment.Offset + count);
count += sizeof({2});
";

        // {0} member Name
        public static string readStringFormat =
@"ushort {0}Len = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
count += sizeof(ushort);
this.{0} = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, {0}Len);
count += {0}Len;
";

        // {0} List Name [capital letter]
        // {1} List Name [small letter]
        public static string readListFormat =
@"this.{1}s.Clear();
ushort {1}sCount = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
count += sizeof(ushort);
for (int i = 0; i < {1}sCount; i++)
{{
    {0} {1} = new {0}();
    {1}.Read(segment, ref count);
    {1}s.Add({1});
}}
";
        // {0} Var Name
        // {1} Type
        public static string writeFormat =
@"Array.Copy(BitConverter.GetBytes({0}), 0, segment.Array, segment.Offset + count, sizeof({1}));
count += sizeof({1});
";

        // {0} member name
        public static string writeStringFormat =
@"ushort {0}Len = (ushort)Encoding.Unicode.GetBytes(this.{0}, 0, this.{0}.Length, segment.Array, segment.Offset + count + sizeof(ushort));
Array.Copy(BitConverter.GetBytes({0}Len), 0, segment.Array, segment.Offset + count, sizeof(ushort));
count += sizeof(ushort);
count += {0}Len;
";

        // {0} List Name [capital letter]
        // {1} List Name [small letter]
        public static string writeListFormat =
@"Array.Copy(BitConverter.GetBytes((ushort){1}s.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
count += sizeof(ushort);
foreach ({0} {1} in {1}s)
    {1}.Write(segment, ref count);
";
    }
}
