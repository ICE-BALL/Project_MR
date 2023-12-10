using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public enum PacketID
{
    SendMsg = 1,
	
}

public interface IPacket
{
    ushort Protocol { get; }
    void Read(ArraySegment<byte> segment);
    ArraySegment<byte> Write();
}


public class SendMsg : IPacket
{
    public ushort Protocol { get { return (ushort)PacketID.SendMsg; } }

    public int UserId;
	public string Msg;
	

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.UserId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		ushort MsgLen = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		this.Msg = Encoding.Unicode.GetString(segment.Array, segment.Offset + count, MsgLen);
		count += MsgLen;
		
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(UserId), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		ushort MsgLen = (ushort)Encoding.Unicode.GetBytes(this.Msg, 0, this.Msg.Length, segment.Array, segment.Offset + count + sizeof(ushort));
		Array.Copy(BitConverter.GetBytes(MsgLen), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		count += MsgLen;
		

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

