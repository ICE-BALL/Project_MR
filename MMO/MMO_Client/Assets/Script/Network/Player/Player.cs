using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int PlayerId;
    int _state = 0;

    public Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
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
        NetworkManager._session.Send(data.Write());
    }
}
