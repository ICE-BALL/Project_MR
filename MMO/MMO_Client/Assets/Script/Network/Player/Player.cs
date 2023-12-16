using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int PlayerId;
    int _state = 0;
    LayerMask _attackMask;
    float _attackRange = 1.5f;

    public Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _attackMask = LayerMask.GetMask("Creature") | LayerMask.GetMask("Player");
    }

    void Update()
    {
        if (_anim != null)
        {
            switch (_state)
            {
                case (int)define.PlayerState.Idle:
                    _anim.SetInteger("State", (int)define.PlayerState.Idle);
                    break;
                case (int)define.PlayerState.Run:
                    _anim.SetInteger("State", (int)define.PlayerState.Run);
                    break;
                case (int)define.PlayerState.Attack:
                    _anim.SetInteger("State", (int)define.PlayerState.Attack);
                    break;
            }
        }
        else
            _anim = GetComponent<Animator>();
    }

    public void PlayerState(int StateSpeed)
    {
        _state = StateSpeed;
    }

    public void OnHit()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward), out hit, _attackRange, _attackMask))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                hit.collider.gameObject.GetComponent<Player>().HitEvent(transform.gameObject);
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Creature"))
                hit.collider.gameObject.GetComponent<Creature>().HitEvent(transform.gameObject);
        }
    }

    public void HitEvent(GameObject obj)
    {
        PlayerStat stat = gameObject.GetComponent<PlayerStat>();
        stat.Hp -= obj.GetComponent<Stat>().Attack;
        Data data = new Data();
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
