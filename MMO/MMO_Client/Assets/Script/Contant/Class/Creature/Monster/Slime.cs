using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Creature
{
    public override void HitEvent(GameObject obj)
    {
        Stat stat = gameObject.GetComponent<Stat>();
        stat.Hp -= obj.GetComponent<Stat>().Attack;
        //Data data = new Data();
        //data.MaxHp = stat.MaxHp;
        //data.Attack = stat.Attack;
        //data.AttackSpeed = stat.AttackSpeed;
        //data.Hp = stat.Hp;
        //data.MaxMp = stat.MaxMp;
        //data.Mp = stat.Mp;
        //data.Speed = stat.Speed;
        //data.PlayerId = stat.PlayerId;
        //NetworkManager._session.Send(data.Write());
    }
}
