using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    protected int _playerId;

    public int PlayerId { get { return _playerId; } set { _playerId = value; } }
}
