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

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _nav = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        switch (_state)
        {
            case define.SliemState.Idle:
                break;
            case define.SliemState.Run:
                break;
            case define.SliemState.Walk:
                break;
            case define.SliemState.Attack:
                break;
        }
    }

    void OnIdle()
    {

    }

    void OnRun()
    {

    }

    void OnWalk()
    {

    }

    void OnAttack()
    {

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
