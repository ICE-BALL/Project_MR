using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public enum MonsterTypes
    {
        Slime,
        TurtleSlime,
    }

    internal class Monster
    {
        #region Data
        #region Network Data
        public int MonsterId { get; set; }
        public int MonsterType { get; set; }
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

        public void SetUp()
        {
            if (MonsterType == (int)MonsterTypes.Slime)
            {
                Random _randX = new Random();
                Random _randZ = new Random();
                Leveling();
                PosX = _randX.Next(-20, 41);
                PosY = 0;
                PosZ = _randZ.Next(40, 66);
            }
        }

        public virtual void Leveling()
        {
            this.Leveling();
        }
    }

    internal class Slime : Monster
    {
        public override void Leveling()
        {
            if (Level == 0)
                Level = 1;

            if (Level == 1)
            {
                MaxHp = 30;
                Hp = MaxHp;
                MaxMp = 0;
                Mp = MaxMp;
                Attack = 3;
                AttackSpeed = 1;
                Speed = 3;
            }
        }
    }
}
