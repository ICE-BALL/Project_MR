using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server
{
    public class RecvPacket
    {
        static RecvPacket _recvPacket = new RecvPacket();
        public static RecvPacket Instance { get { return _recvPacket; } }

        #region Non Server Packet
        public void PlayerList(IPacket packet, ClientSession s)
        {

        }

        public void BroadCastEnter(IPacket packet, ClientSession s)
        {

        }

        public void BroadCastLeave(IPacket packet, ClientSession s)
        {

        }
        #endregion

        public void PlayerMove(IPacket packet, ClientSession s)
        {
            PlayerMove m = packet as PlayerMove;

            for (int i = 0; i < Program.Room._sessions.Count; i++)
            {
                if (Program.Room._sessions[i].SessionId == m.PlayerId)
                {

                    Program.Room._sessions[i].PosX = m.PosX;
                    Program.Room._sessions[i].PosY = m.PosY;
                    Program.Room._sessions[i].PosZ = m.PosZ;

                    Program.Room._sessions[i].RotX = m.RotX;
                    Program.Room._sessions[i].RotY = m.RotY;
                    Program.Room._sessions[i].RotZ = m.RotZ;

                    Program.Room._sessions[i].Map_Zone = m.Map_Zone;
                }
            }

            PlayerMove pm = new PlayerMove();
            pm.PosX = m.PosX;
            pm.PosY = m.PosY;
            pm.PosZ = m.PosZ;

            pm.RotX = m.RotX;
            pm.RotY = m.RotY;
            pm.RotZ = m.RotZ;
            pm.PlayerId = m.PlayerId;
            pm.Map_Zone = m.Map_Zone;
            pm.StateConvertNum = m.StateConvertNum;
            Program.Room.BroadCast(pm.Write(), s);
        }

        public void Data(IPacket packet, ClientSession s)
        {
            Data data = packet as Data;

            s.SessionId = data.PlayerId;
            s.Map_Zone = data.Map_Zone;
            s.Level = data.Level;
            s.MaxHp = data.MaxHp;
            s.Hp = data.Hp;
            s.MaxMp = data.MaxMp;
            s.Mp = data.Mp;
            s.Attack = data.Attack;
            s.AttackSpeed = data.AttackSpeed;
            s.Speed = data.Speed;

            Program.Room.BroadCast(data.Write(), s);
        }

        public void MonsterList(IPacket packet, ClientSession s)
        {
            MonsterList mL = packet as MonsterList;

            for (int i = 0;  i < GameManager.Instance._monsters.Count; i++)
            {
                if (GameManager.Instance._monsters[i] == null || GameManager.Instance._monsters.Count != mL.monsterss.Count)
                    return;

                GameManager.Instance._monsters[i].PosX = mL.monsterss[i].PosX;
                GameManager.Instance._monsters[i].PosY = mL.monsterss[i].PosY;
                GameManager.Instance._monsters[i].PosZ = mL.monsterss[i].PosZ;

                GameManager.Instance._monsters[i].RotX = mL.monsterss[i].RotX;
                GameManager.Instance._monsters[i].RotY = mL.monsterss[i].RotY;
                GameManager.Instance._monsters[i].RotZ = mL.monsterss[i].RotZ;

                GameManager.Instance._monsters[i].MonsterId = mL.monsterss[i].MonsterId;
                GameManager.Instance._monsters[i].Map_Zone = mL.monsterss[i].Map_Zone;
                GameManager.Instance._monsters[i].MonsterType = mL.monsterss[i].MonsterType;
            }

            //Program.Room.BroadCast(mL.Write(), s);
        }

        public void MonsterData(IPacket packet, ClientSession s)
        {
            //MonsterData data = packet as MonsterData;

            //s.Level = data.Level;
            //s.MaxHp = data.MaxHp;
            //s.Hp = data.Hp;
            //s.MaxMp = data.MaxMp;
            //s.Mp = data.Mp;
            //s.Attack = data.Attack;
            //s.AttackSpeed = data.AttackSpeed;
            //s.Speed = data.Speed;

            //Program.Room.BroadCast(data.Write(), s);
        }

        public void MonsterMove(IPacket packet, ClientSession s)
        {
            //MonsterMove m = packet as MonsterMove;

            //for (int i = 0; i < GameManager.Instance._monsters.Count; i++)
            //{
            //    GameManager.Instance._monsters[i].PosX = m.PosX;
            //    GameManager.Instance._monsters[i].PosX = m.PosX;
            //    GameManager.Instance._monsters[i].PosX = m.PosX;

            //}
        }
    }
}
