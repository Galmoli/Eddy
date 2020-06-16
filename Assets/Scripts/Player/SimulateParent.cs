using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulateParent : MonoBehaviour
{
    [HideInInspector]
    public bool _simulate;
    private Transform _targetTransform;
    private Vector3 _offset;
    void Update()
    {
        if (!_simulate) return;
        transform.position = _targetTransform.position + _offset;
    }

    public void InjectTransform(Transform target)
    {
        _targetTransform = target;
        _simulate = true;
        _offset = transform.position - _targetTransform.position;
    }

    public void UnParent()
    {
        _simulate = false;
    }
}
