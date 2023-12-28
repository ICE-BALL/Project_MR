using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Bar : MonoBehaviour
{
    Slider _hpBar;
    public GameObject _target;
    public Stat _targetStat;
    public GameObject _parent;

    private void Start()
    {
        StartCoroutine(FindPlayer());

    }

    private void Update()
    {
        if (_target == null || _hpBar == null || _targetStat == null)
        {
            return;
        }
        else
        {
            if (_parent.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay)
            {
                _hpBar.value = _targetStat.Hp / (float)_targetStat.MaxHp;
            }
            else if (_parent.GetComponent<Canvas>().renderMode == RenderMode.WorldSpace)
            {
                transform.rotation = Camera.main.transform.rotation;
                _hpBar.value = _targetStat.Hp / (float)_targetStat.MaxHp;
            }
        }
        
        

    }

    private IEnumerator FindPlayer()
    {
        while (_target == null)
        {
            _parent = transform.parent.gameObject;
            if (_parent.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay)
            {
                _target = GameObject.FindGameObjectWithTag("Player");
            }
            else if (_parent.GetComponent<Canvas>().renderMode == RenderMode.WorldSpace)
            {
                _parent.GetComponent<Canvas>().worldCamera = Camera.main;
                _target = transform.parent.parent.gameObject;
            }

            yield return null;
        }

        _targetStat = _target.GetComponent<Stat>();
        _hpBar = GetComponent<Slider>();
    }
}
