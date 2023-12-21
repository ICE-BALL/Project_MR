using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Creature
{
    public override void HitEvent(GameObject obj)
    {
        if (obj.gameObject.name == "MyPlayer")
        {
            MonsterStat stat = gameObject.GetComponent<MonsterStat>();
            stat.Hp -= obj.GetComponent<Stat>().Attack;
            MonsterData data = new MonsterData();
            data.Level = stat.Level;
            data.MaxHp = stat.MaxHp;
            data.Attack = stat.Attack;
            data.AttackSpeed = stat.AttackSpeed;
            data.Hp = stat.Hp;
            data.MaxMp = stat.MaxMp;
            data.Mp = stat.Mp;
            data.Speed = stat.Speed;
            data.MonsterId = stat.MonsterId;
            data.Map_Zone = Managers.Game.Map_Zone;
            data.MonsterType = (int)define.MonsterTypes.Slime;
            NetworkManager._session.Send(data.Write());
        }
    }
}
