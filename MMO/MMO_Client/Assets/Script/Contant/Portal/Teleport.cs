using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject _Myplayer;
    GameObject _pressE;

    void Start()
    {
        if (_Myplayer == null)
            _Myplayer = GameObject.Find("MyPlayer");
    }

    void Update()
    {
        if ((_Myplayer.transform.position - transform.position).magnitude < 3)
        {
            if (_pressE == null)
                _pressE = Managers.Resource.Instantiate("UI/UI_Popup/PressE", Managers.UI_Root.transform);
        }
        else
        {
            if (_pressE != null)
            {
                Managers.Resource.Destroy(_pressE);
            }
        }
    }
}
