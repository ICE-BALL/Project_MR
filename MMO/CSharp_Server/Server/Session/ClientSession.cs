using ServerCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ClientSession : PacketSession
    {
        #region Data
        #region Network Data
        public int SessionId { get; set; }
        public int Map_Zone { get; set; }

        public float PosX { get; set; } = 0;
        public float PosY { get; set; } = 0;
        public float PosZ { get; set; } = 0;

        public float RotX { get; set; } = 0;
        public float RotY { get; set; } = 0;
        public float RotZ { get; set; } = 0;
        #endregion
        #region Stat
        public int Level { get; set; }
        public int MaxHp { get; set; }
        public float Hp { get; set; }
        public int MaxMp { get; set; }
        public float Mp { get; set; }
        public float Attack { get; set; }
        public float AttackSpeed { get; set; }
        public float Speed { get; set; }
        #endregion
        #endregion

        public override void OnConnect(EndPoint endPoint)
        {
            Init();
            

            Console.WriteLine($"Connected to {endPoint}");
            Program.Room.Enter(this);

            GameManager.Instance.SetGame(this);
        }

        public override void OnDisconnect(EndPoint endPoint)
        {
            Console.WriteLine($"Disconnected {endPoint}");
            Program.Room.Leave(this);
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
                    PlayerList pL = new PlayerList();
                    pL.Read(Buffer);
                    RecvPacket.Instance.PlayerList(pL, this);
                    break;
                case (ushort)PacketID.BroadCastEnter:
                    BroadCastEnter Be = new BroadCastEnter();
                    Be.Read(Buffer);
                    RecvPacket.Instance.BroadCastEnter(Be, this);
                    break;
                case ((ushort)PacketID.BroadCastLeave):
                    BroadCastLeave Bl = new BroadCastLeave();
                    Bl.Read(Buffer);
                    RecvPacket.Instance.BroadCastLeave(Bl, this);
                    break;
                case (ushort)PacketID.PlayerMove:
                    PlayerMove m = new PlayerMove();
                    m.Read(Buffer);
                    RecvPacket.Instance.PlayerMove(m, this);
                    break;
                case (ushort)PacketID.Data:
                    Data data = new Data();
                    data.Read(Buffer);
                    RecvPacket.Instance.Data(data, this);
                    break;
            }
        }

        public override void OnSend(int numOfbytes)
        {
            //Console.WriteLine($"Sended {numOfbytes} bytes");
        }

        public void Init()
        {
            LevelSystem();
            GameManager.Instance.SetMap(this);
        }

        public void LevelSystem()
        {
            if (Level == 0)
                Level = 1;

            if (Level == 1)
            {
                MaxHp = 100;
                Hp = MaxHp;
                MaxMp = 30;
                Mp = MaxMp;
                Attack = 10;
                AttackSpeed = 0.75f;
                Speed = 5.0f;
            }
            if (Level == 2)
            {
                MaxHp = 120;
                Hp = MaxHp;
                MaxMp = 35;
                Mp = MaxMp;
                Attack = 12;
                AttackSpeed = 0.75f;
                Speed = 5.0f;
            }
        }
    }
}
