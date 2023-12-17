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
    
}
