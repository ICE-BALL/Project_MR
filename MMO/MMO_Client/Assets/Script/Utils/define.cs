using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class define
{
    #region Server

    public enum Map_Zone
    {
        Map_001,
        Map_002,
        Map_003,
        Map_004,
    }

    #endregion

    public enum PlayerState
    {
        Idle,
        Run,
        Attack,
    }

    public enum SliemState
    {
        Idle,
        Run,
        Walk,
        Attack,
    }
}
