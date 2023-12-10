using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    GameObject _player;
    [SerializeField]
    Vector3 _delta;

    public float _mouseX;
    public float _mouseY;
    public float _sensitivity = 1f;

    float _xAngle;
    float _yAngle;

    private void Start()
    {
        _player = GameObject.Find("MyPlayer");

        if (_player == null)
            _player = GameObject.Find("MyPlayer");
        if (_delta == null)
        {
            _delta = new Vector3(0, 2, 0);
        }
    }

    private void LateUpdate()
    {
        if (_player != null)
        {
            if (Input.GetMouseButton(1))
            {
                _mouseX = Input.GetAxis("Mouse X");
                _mouseY = Input.GetAxis("Mouse Y");

                _xAngle += _mouseX * _sensitivity;
                _yAngle -= _mouseY * _sensitivity;
            }
            _yAngle = Mathf.Clamp(_yAngle, -90, 30);

            transform.rotation = Quaternion.Euler(_yAngle, _xAngle, 0);
            transform.position = _player.transform.position + _delta;
        }
        else
            _player = GameObject.FindGameObjectWithTag("Player");
    }
}
