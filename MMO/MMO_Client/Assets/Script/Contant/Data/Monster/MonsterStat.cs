using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : Stat
{
    protected int _monsterId;

    public int MonsterId { get { return _monsterId; } set { _monsterId = value; } }
}
