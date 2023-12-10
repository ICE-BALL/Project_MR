using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Bar : MonoBehaviour
{
    Slider _hpBar;
    public GameObject _player;
    public PlayerStat _playerStat;
    public GameObject _parent;
    private void Start()
    {
        StartCoroutine(FindPlayer());

    }

    private void Update()
    {
        if (_player == null || _hpBar == null || _playerStat == null)
        {
            return;
        }
        else
        {
            if (_parent.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay)
            {
                _hpBar.value = _playerStat.Hp / (float)_playerStat.MaxHp;
            }
            else if (_parent.GetComponent<Canvas>().renderMode == RenderMode.WorldSpace)
            {
                transform.rotation = Camera.main.transform.rotation;
                _hpBar.value = _playerStat.Hp / (float)_playerStat.MaxHp;
            }
        }
        
        

    }

    private IEnumerator FindPlayer()
    {
        while (_player == null)
        {
            _parent = transform.parent.gameObject;
            if (_parent.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay)
            {
                _player = GameObject.FindGameObjectWithTag("Player");
            }
            else if (_parent.GetComponent<Canvas>().renderMode == RenderMode.WorldSpace)
            {
                _parent.GetComponent<Canvas>().worldCamera = Camera.main;
                _player = transform.parent.parent.gameObject;
            }

            yield return null;
        }

        _playerStat = _player.GetComponent<PlayerStat>();
        _hpBar = GetComponent<Slider>();
    }
}
