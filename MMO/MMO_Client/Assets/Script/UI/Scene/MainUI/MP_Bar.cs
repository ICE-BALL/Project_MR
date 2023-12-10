using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MP_Bar : MonoBehaviour
{
    public Slider _mpBar;
    public GameObject _player;
    public PlayerStat _playerStat;

    private void Start()
    {
        StartCoroutine(FindPlayer());
    }

    private void Update()
    {
        if (_playerStat != null && _player != null && _mpBar != null)
            _mpBar.value = _playerStat.Mp / (float)_playerStat.MaxMp;
    }

    private IEnumerator FindPlayer()
    {
        while (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");

            yield return null;
        }

        _playerStat = _player.GetComponent<PlayerStat>();
        _mpBar = GetComponent<Slider>();
    }
}
