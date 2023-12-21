using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    protected int _playerId;

    public int PlayerId { get { return _playerId; } set { _playerId = value; } }

    LayerMask _attackMask;
    float _attackRange = 1.5f;

    private void Start()
    {
        _attackMask = LayerMask.GetMask("Creature") | LayerMask.GetMask("Player");
    }

    public void OnHit()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward), out hit, _attackRange, _attackMask))
        {
            if (gameObject.transform.tag == "Player")
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                    hit.collider.gameObject.GetComponent<PlayerStat>().HitEvent(transform.gameObject);
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Creature"))
                    hit.collider.gameObject.GetComponent<Creature>().HitEvent(transform.gameObject);
            }
        }
    }

    public void HitEvent(GameObject obj)
    {
        if (obj.gameObject.name == "MyPlayer")
        {
            PlayerStat stat = gameObject.GetComponent<PlayerStat>();
            stat.Hp -= obj.GetComponent<Stat>().Attack;
            Data data = new Data();
            data.Level = stat.Level;
            data.MaxHp = stat.MaxHp;
            data.Attack = stat.Attack;
            data.AttackSpeed = stat.AttackSpeed;
            data.Hp = stat.Hp;
            data.MaxMp = stat.MaxMp;
            data.Mp = stat.Mp;
            data.Speed = stat.Speed;
            data.PlayerId = stat.PlayerId;
            data.Map_Zone = Managers.Game.Map_Zone;
            NetworkManager._session.Send(data.Write());
        }


    }
}
