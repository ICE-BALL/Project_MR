using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : Stat
{
    private void Start()
    {
        if (Level <= 0)
        {
            Speed = 3.0f;
        }
    }
}
