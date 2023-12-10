using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public float _minDistance = 1f;
    public float _maxDistance = 4f;
    public float _smooth = 10.0f;

    Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;

    public float distance;
    public float A = 0.03f;
    public float _mouseWheel;

    private void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    void Update()
    {
        MouseWheel();
        CameraCollisionUpdate();
    }

    void CameraCollisionUpdate()
    {
        Vector3 DCameraPos = transform.parent.TransformPoint(dollyDir * _maxDistance);
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, DCameraPos, out hit))
        {
            distance = Mathf.Clamp((hit.distance * A), _minDistance, _maxDistance);
        }
        else
        {
            distance = _maxDistance;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * _smooth);
    }

    void MouseWheel()
    {
        _mouseWheel = -(Input.GetAxis("Mouse ScrollWheel") * 10);
        if (_maxDistance >= 8)
        {
            if (_mouseWheel < 0)
            {
                _maxDistance += _mouseWheel;
            }
        }
        else if (_maxDistance <= 4)
        {
            if (_mouseWheel > 0)
            {
                _maxDistance += _mouseWheel;
            }
        }
        else
        {
            _maxDistance += _mouseWheel;
        }
    }
}
