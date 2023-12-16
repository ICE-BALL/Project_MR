using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class GameRoom
    {
        public List<ClientSession> _sessions = new List<ClientSession>();

        object _lock = new object();

        public void BroadCast(ArraySegment<byte> Buffer, ClientSession s = null)
        {
            lock (_lock)
            {
                foreach (ClientSession session in _sessions)
                {
                    if (session != null)
                        if (s == session)
                            continue;
                    session.Send(Buffer);
                }
            }
        }

        public void Enter(ClientSession session)
        {
            lock (_lock)
            {
                _sessions.Add(session);

                PlayerList list = new PlayerList();
                foreach (ClientSession s in _sessions)
                {
                    PlayerList.Player p = new PlayerList.Player()
                    {
                        IsSelf = (s == session),
                        PlayerId = s.SessionId,
                        Map_Zone = s.Map_Zone,
                        PosX = s.PosX,
                        PosY = s.PosY,
                        PosZ = s.PosZ,

                        RotX = s.RotX,
                        RotY = s.RotY,
                        RotZ = s.RotZ,
                    };
                    list.players.Add(p);
                }
                ArraySegment<byte> buff = list.Write();
                session.Send(buff);

                Data data = new Data();
                data.PlayerId = session.SessionId;
                data.Map_Zone = session.Map_Zone;
                data.Level = session.Level;
                data.MaxHp = session.MaxHp;
                data.Hp = session.Hp;
                data.MaxMp = session.MaxMp;
                data.Mp = session.Mp;
                data.Attack = session.Attack;
                data.AttackSpeed = session.AttackSpeed;
                data.Speed = session.Speed;
                session.Send(data.Write());

                if (_sessions.Count > 1)
                {
                    BroadCastEnter enter = new BroadCastEnter();
                    enter.PlayerId = session.SessionId;
                    enter.Map_Zone = session.Map_Zone;
                    enter.PosX = session.PosX;
                    enter.PosY = session.PosY;
                    enter.PosZ = session.PosZ;

                    enter.RotX = session.RotX;
                    enter.RotY = session.RotY;
                    enter.RotZ = session.RotZ;

                    BroadCast(enter.Write(), session);
                    BroadCast(data.Write(), session);
                }
            }
            
        }

        public void Leave(ClientSession s)
        {
            lock (_lock)
            {
                BroadCastLeave bl = new BroadCastLeave();
                bl.PlayerId = s.SessionId;
                bl.Map_Zone = s.Map_Zone;

                BroadCast(bl.Write(), s);

                _sessions.Remove(s);
            }
        }
    }
}
