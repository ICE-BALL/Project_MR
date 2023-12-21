using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class GameManager
    {
        static GameManager _instance = new GameManager();
        public static GameManager Instance { get { return _instance; } }

        #region Monster
        public List<Monster> _monsters = new List<Monster>();
        public int _monsterId = 0;
        public int _monsterCount = 0;
        public int MaxMonster = 0;
        #endregion

        public enum Map_Zone
        {
            Map_001,
            Map_002,
            Map_003,
            Map_004,
        }

        public void SetMap(ClientSession s)
        {
            if (s.Map_Zone == 0)
                s.Map_Zone = (int)Map_Zone.Map_001;
        }

        public void SetGame(ClientSession s)
        {
            switch (s.Map_Zone)
            {
                case (int)Map_Zone.Map_001:
                    MaxMonster = 10;
                    SetMonster(MonsterTypes.Slime, s.Map_Zone);
                    break;
                case (int)Map_Zone.Map_002:
                    break;
                case (int)Map_Zone.Map_003:
                    break;
                case (int)Map_Zone.Map_004:
                    break;
            }
        }

        public void SetMonster(MonsterTypes types, int mapZone)
        {
            for (int i = 0; _monsterCount < MaxMonster; i++)
            {
                switch (types)
                {
                    case MonsterTypes.Slime:
                        Slime slime = new Slime();
                        slime.MonsterType = (int)MonsterTypes.Slime;
                        slime.SetUp();
                        slime.MonsterId = ++_monsterId;
                        _monsters.Add(slime);
                        break;
                    case MonsterTypes.TurtleSlime:
                        break;
                }
                _monsterCount += 1;
            }

            MonsterList mList = new MonsterList();
            foreach (Monster monster in _monsters)
            {
                MonsterList.Monsters m = new MonsterList.Monsters();
                m.RotX = monster.RotX;
                m.RotY = monster.RotY;
                m.RotZ = monster.RotZ;
                m.PosX = monster.PosX;
                m.PosY = monster.PosY;
                m.PosZ = monster.PosZ;
                m.MonsterType = monster.MonsterType;
                m.MonsterId = monster.MonsterId;
                m.Map_Zone = mapZone;

                mList.monsterss.Add(m);
            }
            Program.Room.BroadCast(mList.Write());

            foreach (Monster monster in _monsters)
            {
                MonsterData data = new MonsterData();
                data.Level = monster.Level;
                data.MaxHp = monster.MaxHp;
                data.Hp = monster.Hp;
                data.MaxMp = monster.MaxMp;
                data.Mp = monster.Mp;
                data.Attack = monster.Attack;
                data.AttackSpeed = monster.AttackSpeed;
                data.Speed = monster.Speed;
                data.MonsterType = monster.MonsterType;
                data.MonsterId = monster.MonsterId;
                data.Map_Zone = mapZone;
                Program.Room.BroadCast(data.Write());
            }
        }
    }
}
