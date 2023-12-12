using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected int _level;
    [SerializeField]
    protected int _maxHp;
    [SerializeField]
    protected float _hp;
    [SerializeField]
    protected int _maxMp;
    [SerializeField]
    protected float _mp;
    [SerializeField]
    protected float _attack;
    [SerializeField]
    protected float _attackSpeed;

    [SerializeField]
    protected float _speed;

    public int Level { get { return _level; } set { _level = value; } }
    public int MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public float Hp { get { return _hp; } set { _hp = value; } }
    public int MaxMp { get { return _maxMp; } set { _maxMp = value; } }
    public float Mp { get { return _mp; } set { _mp = value; } }
    public float Attack { get { return _attack; } set { _attack = value; } }
    public float AttackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }

    public float Speed { get { return _speed; } set { _speed = value; } }
}
