using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeController : MonoBehaviour
{
    define.SliemState _state = define.SliemState.Idle;
    GameObject _target;
    Animator _anim;
    NavMeshAgent _nav;
    MonsterStat _stat;

    public float _scanRange = 10.0f;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _nav = GetComponent<NavMeshAgent>();
        _stat = GetComponent<MonsterStat>();
        GetComponent<Slime>().SetDesPos(transform.position);
    }

    private void Update()
    {
        UpdateState();
        MonsterAI();
    }

    void UpdateState()
    {
        switch (_state)
        {
            case define.SliemState.Idle:
                _anim.SetInteger("state", (int)_state);
                break;
            case define.SliemState.Run:
                _anim.SetInteger("state", (int)_state);
                break;
            case define.SliemState.Walk:
                _anim.SetInteger("state", (int)_state);
                break;
            case define.SliemState.Attack:
                _anim.SetInteger("state", (int)_state);
                break;
        }
    }

    void MonsterAI()
    {
        TargetingSystem();
        if ((transform.position - _target.transform.position).magnitude < _scanRange)
        {
            if ((transform.position - _target.transform.position).magnitude < 2f)
            {
                _state = define.SliemState.Idle;
                _nav.SetDestination(transform.position);
            }
            else
            {
                _state = define.SliemState.Run;
                _nav.SetDestination(_target.transform.position);
                _nav.speed = _stat.Speed;
                transform.LookAt(_target.transform.position);
            }
        }
        else
        {
            _state = define.SliemState.Idle;
            _nav.speed = _stat.Speed;
            _nav.SetDestination(transform.position);
        }
    }

    void TargetingSystem()
    {
        foreach (GameObject target in PlayerManager.Instance._players)
        {
            if (target == null)
                continue;

            if (_target == null)
            {
                _target = target;
                continue;
            }

            // target is more closer
            if ((_target.transform.position - transform.position).magnitude > (target.transform.position - transform.position).magnitude)
            {
                _target = target;
            }

        }
    }
}
