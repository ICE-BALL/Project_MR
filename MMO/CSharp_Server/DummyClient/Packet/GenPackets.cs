using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public enum PacketID
{
    PlayerList = 2,
	BroadCastEnter = 3,
	BroadCastLeave = 4,
	PlayerMove = 5,
	Data = 6,
	MonsterList = 8,
	MonsterData = 9,
	
}

public enum PacketType
{
    Send,
    BroadCastSend,
}

public interface IPacket
{
    ushort Protocol { get; }
    void Read(ArraySegment<byte> segment);
    ArraySegment<byte> Write();
}


public class PlayerList : IPacket
{
    public ushort Protocol { get { return (ushort)PacketID.PlayerList; } }

    
	public class Player
	{
	    public int PlayerId;
		public int Map_Zone;
		public bool IsSelf;
		public float PosX;
		public float PosY;
		public float PosZ;
		public float RotX;
		public float RotY;
		public float RotZ;
		
	
	    public void Read(ArraySegment<byte> segment, ref ushort count)
	    {
	        this.PlayerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
			this.Map_Zone = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
			this.IsSelf = BitConverter.ToBoolean(segment.Array, segment.Offset + count);
			count += sizeof(bool);
			this.PosX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
			count += sizeof(float);
			this.PosY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
			count += sizeof(float);
			this.PosZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
			count += sizeof(float);
			this.RotX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
			count += sizeof(float);
			this.RotY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
			count += sizeof(float);
			this.RotZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
			count += sizeof(float);
			
	    }
	
	    public void Write(ArraySegment<byte> segment, ref ushort count)
	    {
	        Array.Copy(BitConverter.GetBytes(PlayerId), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
			Array.Copy(BitConverter.GetBytes(Map_Zone), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
			Array.Copy(BitConverter.GetBytes(IsSelf), 0, segment.Array, segment.Offset + count, sizeof(bool));
			count += sizeof(bool);
			Array.Copy(BitConverter.GetBytes(PosX), 0, segment.Array, segment.Offset + count, sizeof(float));
			count += sizeof(float);
			Array.Copy(BitConverter.GetBytes(PosY), 0, segment.Array, segment.Offset + count, sizeof(float));
			count += sizeof(float);
			Array.Copy(BitConverter.GetBytes(PosZ), 0, segment.Array, segment.Offset + count, sizeof(float));
			count += sizeof(float);
			Array.Copy(BitConverter.GetBytes(RotX), 0, segment.Array, segment.Offset + count, sizeof(float));
			count += sizeof(float);
			Array.Copy(BitConverter.GetBytes(RotY), 0, segment.Array, segment.Offset + count, sizeof(float));
			count += sizeof(float);
			Array.Copy(BitConverter.GetBytes(RotZ), 0, segment.Array, segment.Offset + count, sizeof(float));
			count += sizeof(float);
			
	    }
	}
	public List<Player> players = new List<Player>();
	

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.players.Clear();
		ushort playersCount = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		for (int i = 0; i < playersCount; i++)
		{
		    Player player = new Player();
		    player.Read(segment, ref count);
		    players.Add(player);
		}
		
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes((ushort)players.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		foreach (Player player in players)
		    player.Write(segment, ref count);
		

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class BroadCastEnter : IPacket
{
    public ushort Protocol { get { return (ushort)PacketID.BroadCastEnter; } }

    public int PlayerId;
	public int Map_Zone;
	public float PosX;
	public float PosY;
	public float PosZ;
	public float RotX;
	public float RotY;
	public float RotZ;
	

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.PlayerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.Map_Zone = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.PosX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.PosY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.PosZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.RotX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.RotY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.RotZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(PlayerId), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(Map_Zone), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(PosX), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(PosY), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(PosZ), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(RotX), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(RotY), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(RotZ), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class BroadCastLeave : IPacket
{
    public ushort Protocol { get { return (ushort)PacketID.BroadCastLeave; } }

    public int PlayerId;
	public int Map_Zone;
	

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.PlayerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.Map_Zone = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(PlayerId), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(Map_Zone), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class PlayerMove : IPacket
{
    public ushort Protocol { get { return (ushort)PacketID.PlayerMove; } }

    public int PlayerId;
	public int StateConvertNum;
	public int Map_Zone;
	public float PosX;
	public float PosY;
	public float PosZ;
	public float RotX;
	public float RotY;
	public float RotZ;
	

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.PlayerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.StateConvertNum = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.Map_Zone = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.PosX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.PosY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.PosZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.RotX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.RotY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.RotZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(PlayerId), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(StateConvertNum), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(Map_Zone), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(PosX), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(PosY), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(PosZ), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(RotX), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(RotY), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(RotZ), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class Data : IPacket
{
    public ushort Protocol { get { return (ushort)PacketID.Data; } }

    public int PlayerId;
	public int Map_Zone;
	public int Level;
	public int MaxHp;
	public float Hp;
	public int MaxMp;
	public float Mp;
	public float Attack;
	public float AttackSpeed;
	public float Speed;
	

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.PlayerId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.Map_Zone = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.Level = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.MaxHp = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.Hp = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.MaxMp = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.Mp = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.Attack = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.AttackSpeed = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.Speed = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(PlayerId), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(Map_Zone), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(Level), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(MaxHp), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(Hp), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(MaxMp), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(Mp), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(Attack), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(AttackSpeed), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(Speed), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class MonsterList : IPacket
{
    public ushort Protocol { get { return (ushort)PacketID.MonsterList; } }

    
	public class Monsters
	{
	    public int MonsterId;
		public int Map_Zone;
		public float PosX;
		public float PosY;
		public float PosZ;
		public float RotX;
		public float RotY;
		public float RotZ;
		
	
	    public void Read(ArraySegment<byte> segment, ref ushort count)
	    {
	        this.MonsterId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
			this.Map_Zone = BitConverter.ToInt32(segment.Array, segment.Offset + count);
			count += sizeof(int);
			this.PosX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
			count += sizeof(float);
			this.PosY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
			count += sizeof(float);
			this.PosZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
			count += sizeof(float);
			this.RotX = BitConverter.ToSingle(segment.Array, segment.Offset + count);
			count += sizeof(float);
			this.RotY = BitConverter.ToSingle(segment.Array, segment.Offset + count);
			count += sizeof(float);
			this.RotZ = BitConverter.ToSingle(segment.Array, segment.Offset + count);
			count += sizeof(float);
			
	    }
	
	    public void Write(ArraySegment<byte> segment, ref ushort count)
	    {
	        Array.Copy(BitConverter.GetBytes(MonsterId), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
			Array.Copy(BitConverter.GetBytes(Map_Zone), 0, segment.Array, segment.Offset + count, sizeof(int));
			count += sizeof(int);
			Array.Copy(BitConverter.GetBytes(PosX), 0, segment.Array, segment.Offset + count, sizeof(float));
			count += sizeof(float);
			Array.Copy(BitConverter.GetBytes(PosY), 0, segment.Array, segment.Offset + count, sizeof(float));
			count += sizeof(float);
			Array.Copy(BitConverter.GetBytes(PosZ), 0, segment.Array, segment.Offset + count, sizeof(float));
			count += sizeof(float);
			Array.Copy(BitConverter.GetBytes(RotX), 0, segment.Array, segment.Offset + count, sizeof(float));
			count += sizeof(float);
			Array.Copy(BitConverter.GetBytes(RotY), 0, segment.Array, segment.Offset + count, sizeof(float));
			count += sizeof(float);
			Array.Copy(BitConverter.GetBytes(RotZ), 0, segment.Array, segment.Offset + count, sizeof(float));
			count += sizeof(float);
			
	    }
	}
	public List<Monsters> monsterss = new List<Monsters>();
	

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.monsterss.Clear();
		ushort monsterssCount = BitConverter.ToUInt16(segment.Array, segment.Offset + count);
		count += sizeof(ushort);
		for (int i = 0; i < monsterssCount; i++)
		{
		    Monsters monsters = new Monsters();
		    monsters.Read(segment, ref count);
		    monsterss.Add(monsters);
		}
		
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes((ushort)monsterss.Count), 0, segment.Array, segment.Offset + count, sizeof(ushort));
		count += sizeof(ushort);
		foreach (Monsters monsters in monsterss)
		    monsters.Write(segment, ref count);
		

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

public class MonsterData : IPacket
{
    public ushort Protocol { get { return (ushort)PacketID.MonsterData; } }

    public int MonsterId;
	public int Map_Zone;
	public int Level;
	public int MaxHp;
	public float Hp;
	public int MaxMp;
	public float Mp;
	public float Attack;
	public float AttackSpeed;
	public float Speed;
	

    public void Read(ArraySegment<byte> segment)
    {
        ushort count = 0;
        count += sizeof(ushort);
        count += sizeof(ushort);

        this.MonsterId = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.Map_Zone = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.Level = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.MaxHp = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.Hp = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.MaxMp = BitConverter.ToInt32(segment.Array, segment.Offset + count);
		count += sizeof(int);
		this.Mp = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.Attack = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.AttackSpeed = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		this.Speed = BitConverter.ToSingle(segment.Array, segment.Offset + count);
		count += sizeof(float);
		
    }

    public ArraySegment<byte> Write()
    {
        ArraySegment<byte> segment = SendBufferHelper.Open(4096);
        ushort count = 0;
        count += sizeof(ushort);
        Array.Copy(BitConverter.GetBytes(Protocol), 0, segment.Array, segment.Offset + count, sizeof(ushort));
        count += sizeof(ushort);

        Array.Copy(BitConverter.GetBytes(MonsterId), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(Map_Zone), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(Level), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(MaxHp), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(Hp), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(MaxMp), 0, segment.Array, segment.Offset + count, sizeof(int));
		count += sizeof(int);
		Array.Copy(BitConverter.GetBytes(Mp), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(Attack), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(AttackSpeed), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		Array.Copy(BitConverter.GetBytes(Speed), 0, segment.Array, segment.Offset + count, sizeof(float));
		count += sizeof(float);
		

        Array.Copy(BitConverter.GetBytes(count), 0, segment.Array, segment.Offset, sizeof(ushort));

        return SendBufferHelper.Close(count);
    }
}

