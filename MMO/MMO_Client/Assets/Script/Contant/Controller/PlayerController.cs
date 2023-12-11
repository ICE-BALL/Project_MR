using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerStat _stat;
    Vector3[] _dirs = new Vector3[4];
    GameObject _camera;
    LayerMask _mask;
    Animator _anim;

    public define.PlayerState _state = define.PlayerState.Idle;

    void Start()
    {
        _mask = LayerMask.GetMask("Creature");
        _stat = GetComponent<PlayerStat>();
        _camera = Camera.main.gameObject;
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        switch (_state)
        {
            case define.PlayerState.Idle:
                _anim.SetInteger("State", (int)define.PlayerState.Idle);
                break;
            case define.PlayerState.Run:
                _anim.SetInteger("State", (int)define.PlayerState.Run);
                break;
            case define.PlayerState.Attack:
                _anim.SetInteger("State", (int)define.PlayerState.Attack);
                break;
        }
        PlayerMovement();
        Debug.DrawRay(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward), Color.red);

    }

    void PlayerMovement()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _stat.Hp -= 10;
            _stat.Mp -= 1;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A) || Input.GetMouseButton(0))
        {
            if (_state == define.PlayerState.Attack)
                return;

            _dirs[0] = new Vector3(_camera.transform.forward.x, 0f, _camera.transform.forward.z).normalized;
            _dirs[1] = new Vector3(-_camera.transform.forward.x, 0f, -_camera.transform.forward.z).normalized;
            _dirs[2] = new Vector3(_camera.transform.right.x, 0f, _camera.transform.right.z).normalized;
            _dirs[3] = new Vector3(-_camera.transform.right.x, 0f, -_camera.transform.right.z).normalized;

            _state = define.PlayerState.Run;
            if (Input.GetKey(KeyCode.W))
            {
                if (Physics.Raycast(transform.position + Vector3.up, _dirs[0], 0.5f/*, _mask*/) == false)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_dirs[0]), 0.5f);
                    transform.position += _dirs[0] * _stat.Speed * Time.deltaTime;
                }

            }
            if (Input.GetKey(KeyCode.S))
            {
                if (Physics.Raycast(transform.position + Vector3.up, _dirs[1], 0.5f/*, _mask*/) == false)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_dirs[1]), 0.5f);
                    transform.position += _dirs[1] * _stat.Speed * Time.deltaTime;
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (Physics.Raycast(transform.position + Vector3.up, _dirs[2], 0.5f/*, _mask*/) == false)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_dirs[2]), 0.5f);
                    transform.position += _dirs[2] * _stat.Speed * Time.deltaTime;
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                if (Physics.Raycast(transform.position + Vector3.up, _dirs[3], 0.5f/*, _mask*/) == false)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_dirs[3]), 0.5f);
                    transform.position += _dirs[3] * _stat.Speed * Time.deltaTime;
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (_state != define.PlayerState.Attack)
                {
                    _state = define.PlayerState.Attack;
                    StartCoroutine("CoAttack");
                }
            }
        }
        else if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)))
            if (_state != define.PlayerState.Attack)
                _state = define.PlayerState.Idle;
    }

    IEnumerator CoAttack()
    {
        yield return new WaitForSeconds(_stat.AttackSpeed);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            _state = define.PlayerState.Run;
        }
        else if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)))
        {
            _state = define.PlayerState.Idle;
        }

    }

    public void OnHit()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward), out hit, 1.5f, _mask))
        {
            PlayerStat stat = hit.collider.gameObject.GetComponent<PlayerStat>();
            if (stat != null)
            {
                stat.Hp -= _stat.Attack;
                Data data = new Data();
                data.MaxHp = stat.MaxHp;
                data.Attack = stat.Attack;
                data.AttackSpeed = stat.AttackSpeed;
                data.Hp = stat.Hp;
                data.MaxMp = stat.MaxMp;
                data.Mp = stat.Mp;
                data.Speed = stat.Speed;
                data.PlayerId = stat.PlayerId;
                NetworkManager._session.Send(data.Write());
            }
        }
    }
}
