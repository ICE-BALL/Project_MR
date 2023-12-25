using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerList;

public class RecvPacket
{
    static RecvPacket _instance = new RecvPacket();
    public static RecvPacket Instance { get { return _instance; } }

    public MyPlayer _myPlayer;

    // Recv Functions

    /* ********************************************************************************************************************** */

    public void Add(IPacket packet)
    {
        if (packet.Protocol != (int)PacketID.PlayerList)
            return;

        PlayerList pL = packet as PlayerList;

        if (pL == null)
        {
            Debug.Log("Null PlayerList");
            return;
        }

        foreach (PlayerList.Player p in pL.players)
        {
            GameObject go;

            if (p.IsSelf)
            {
                go = Managers.Resource.Instantiate("Game/Player");
                go.name = "MyPlayer";
                go.tag = "Player";
                MyPlayer myPlayer = go.AddComponent<MyPlayer>();
                myPlayer.gameObject.AddComponent<PlayerController>();
                myPlayer.gameObject.AddComponent<PlayerStat>();
                Managers.Resource.Instantiate("UI/UI_Scene/UI_HpBar", myPlayer.transform);
                myPlayer.PlayerId = p.PlayerId;
                myPlayer.transform.position = new Vector3(p.PosX, p.PosY, p.PosZ);
                myPlayer.transform.rotation = Quaternion.Euler(new Vector3(p.RotX, p.RotY, p.RotZ));
                _myPlayer = myPlayer;
                NetworkManager._session.SessionId = p.PlayerId;
                PlayerManager.Instance._players.Add(_myPlayer.gameObject);
            }
            else
            {
                if (p.Map_Zone == Managers.Game.Map_Zone)
                {
                    go = Managers.Resource.Instantiate("Game/Player");
                    Player player = go.AddComponent<Player>();
                    Managers.Resource.Instantiate("UI/UI_Scene/UI_HpBar", player.transform);
                    player.gameObject.AddComponent<PlayerStat>();
                    player.PlayerId = p.PlayerId;
                    player.transform.position = new Vector3(p.PosX, p.PosY, p.PosZ);
                    player.transform.rotation = Quaternion.Euler(new Vector3(p.RotX, p.RotY, p.RotZ));
                    NetworkManager._players.Add(p.PlayerId, player);
                    PlayerManager.Instance._players.Add(player.gameObject);
                }
            }
        }
    }

    public void AddOther(IPacket packet)
    {
        BroadCastEnter enter = packet as BroadCastEnter;

        GameObject go;

        if (enter.Map_Zone == Managers.Game.Map_Zone)
        {
            go = Managers.Resource.Instantiate("Game/Player");
            Player player = go.AddComponent<Player>();
            Managers.Resource.Instantiate("UI/UI_Scene/UI_HpBar", player.transform);
            player.gameObject.AddComponent<PlayerStat>();
            player.PlayerId = enter.PlayerId;
            player.transform.position = new Vector3(enter.PosX, enter.PosY, enter.PosZ);
            player.transform.rotation = Quaternion.Euler(new Vector3(enter.RotX, enter.RotY, enter.RotZ));
            NetworkManager._players.Add(enter.PlayerId, player);
            PlayerManager.Instance._players.Add(player.gameObject);
        }
    }

    public void MoveOtherPlayer(IPacket packet)
    {
        PlayerMove pm = packet as PlayerMove;

        if (pm.Map_Zone == Managers.Game.Map_Zone)
        {
            Player p;
            if (NetworkManager._players.TryGetValue(pm.PlayerId, out p) == true)
            {
                p.gameObject.transform.position = new Vector3(pm.PosX, pm.PosY, pm.PosZ);
                p.gameObject.transform.rotation = Quaternion.Euler(new Vector3(pm.RotX, pm.RotY, pm.RotZ));
                p.PlayerState(pm.StateConvertNum);
            }
        }
    }

    public void Leave(IPacket packet)
    {
        BroadCastLeave leave = packet as BroadCastLeave;

        if (leave.Map_Zone == Managers.Game.Map_Zone)
        {
            Player p;
            if (NetworkManager._players.TryGetValue(leave.PlayerId, out p))
            {
                Managers.Resource.Destroy(p.gameObject);
                PlayerManager.Instance._players.Remove(p.gameObject);

                NetworkManager._players.Remove(leave.PlayerId);
            }
        }

    }

    public void SetData(IPacket packet)
    {
        Data data = packet as Data;

        if (_myPlayer.PlayerId == data.PlayerId)
        {
            PlayerStat stat = _myPlayer.gameObject.GetComponent<PlayerStat>();
            stat.Level = data.Level;
            stat.MaxHp = data.MaxHp;
            stat.Hp = data.Hp;
            stat.MaxMp = data.MaxMp;
            stat.Mp = data.Mp;
            stat.Attack = data.Attack;
            stat.AttackSpeed = data.AttackSpeed;
            stat.Speed = data.Speed;
            stat.PlayerId = data.PlayerId;
        }
        else
        {
            if (data.Map_Zone == Managers.Game.Map_Zone)
            {
                Player p;
                if (NetworkManager._players.TryGetValue(data.PlayerId, out p))
                {
                    PlayerStat stat = p.gameObject.GetComponent<PlayerStat>();
                    stat.Level = data.Level;
                    stat.MaxHp = data.MaxHp;
                    stat.Hp = data.Hp;
                    stat.MaxMp = data.MaxMp;
                    stat.Mp = data.Mp;
                    stat.Attack = data.Attack;
                    stat.AttackSpeed = data.AttackSpeed;
                    stat.Speed = data.Speed;
                    stat.PlayerId = data.PlayerId;
                }
            }
        }
    }

    public void SetMonsterData(IPacket packet)
    {
        MonsterData data = packet as MonsterData;

        if (data.Map_Zone == Managers.Game.Map_Zone)
        {
            Creature creature;
            if (NetworkManager._monsters.TryGetValue(data.MonsterId, out creature))
            {
                MonsterStat stat = creature.gameObject.GetComponent<MonsterStat>();
                stat.Level = data.Level;
                stat.MaxHp = data.MaxHp;
                stat.Hp = data.Hp;
                stat.MaxMp = data.MaxMp;
                stat.Mp = data.Mp;
                stat.Attack = data.Attack;
                stat.AttackSpeed = data.AttackSpeed;
                stat.Speed = data.Speed;
                stat.MonsterId = data.MonsterId;
            }
        }
    }

    public void AddMonster(IPacket packet)
    {
        MonsterList mList = packet as MonsterList;

        if (NetworkManager._monsters.Count > 0)
        {
            for (int i = 0; i <= NetworkManager._monsters.Count; i++)
            {
                if (mList.monsterss.Count <= i)
                    return;
                if (mList.monsterss[i] == null || NetworkManager._monsters.Count != mList.monsterss.Count)
                    continue;

                if (mList.monsterss[i].Map_Zone != Managers.Game.Map_Zone)
                    continue;

                if (NetworkManager._monsters.Count != mList.monsterss.Count)
                    return;

                Creature c;
                if (NetworkManager._monsters.TryGetValue(i, out c))
                {
                    c.transform.position = new Vector3(mList.monsterss[i].PosX, mList.monsterss[i].PosY, mList.monsterss[i].PosZ);
                    c.transform.rotation = Quaternion.Euler(new Vector3(mList.monsterss[i].RotX, mList.monsterss[i].RotY, mList.monsterss[i].RotZ));
                    c.GetComponent<MonsterStat>().MonsterId = mList.monsterss[i].MonsterId;
                }
            }
        }
        else
        {
            foreach (MonsterList.Monsters m in mList.monsterss)
            {
                if (m.Map_Zone != Managers.Game.Map_Zone)
                    continue;

                string monster = "";
                switch (m.MonsterType)
                {
                    case (int)define.MonsterTypes.Slime:
                        monster = "Slime";
                        break;
                    case (int)define.MonsterTypes.TurtleSlime:
                        monster = "TurtleSlime";
                        break;
                }

                GameObject go;
                go = Managers.Resource.Instantiate($"Game/Monster/{monster}");
                if (go == null)
                    return;

                switch (m.MonsterType)
                {
                    case (int)define.MonsterTypes.Slime:
                        go.AddComponent<Slime>();
                        break;
                    case (int)define.MonsterTypes.TurtleSlime:
                        break;
                }
                go.transform.position = new Vector3(m.PosX, m.PosY, m.PosZ);
                go.transform.rotation = Quaternion.Euler(new Vector3(m.RotX, m.RotY, m.RotZ));
                NetworkManager._monsters.Add(m.MonsterId, go.GetComponent<Slime>());
            }
        }
        
    }


    /* *********************************************************************************************************************** */

    // Functions
}
