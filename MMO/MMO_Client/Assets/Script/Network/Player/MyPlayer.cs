using ServerCore;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyPlayer : Player
{
    PlayerStat _stat;

    void Start()
    {
        _stat = GetComponent<PlayerStat>();
    }

    void Update()
    {
        UpdateMovePacket();
        UpdateDataPacket();
        Managers.Game.SendMovePacket();
    }

    public void UpdateMovePacket()
    {
        PlayerMove movePacket = new PlayerMove();
        movePacket.PlayerId = PlayerId;
        movePacket.Map_Zone = Managers.Game.Map_Zone;
        movePacket.PosX = transform.position.x;
        movePacket.PosY = transform.position.y;
        movePacket.PosZ = transform.position.z;

        movePacket.RotX = transform.eulerAngles.x;
        movePacket.RotY = transform.eulerAngles.y;
        movePacket.RotZ = transform.eulerAngles.z;

        movePacket.StateConvertNum = (int)GetComponent<PlayerController>()._state;
        
        NetworkManager._session.Send(movePacket.Write());
    }

    public void UpdateDataPacket()
    {
        Data data = new Data();
        data.PlayerId = PlayerId;
        data.Map_Zone = Managers.Game.Map_Zone;
        data.Level = _stat.Level;
        data.MaxHp = _stat.MaxHp;
        data.Hp = _stat.Hp;
        data.MaxMp = _stat.MaxMp;
        data.Mp = _stat.Mp;
        data.Attack = _stat.Attack;
        data.AttackSpeed = _stat.AttackSpeed;
        data.Speed = _stat.Speed;

        NetworkManager._session.Send(data.Write());
    }
}
