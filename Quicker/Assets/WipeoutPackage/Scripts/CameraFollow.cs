using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector3 _offset;


    Transform _targetToFollow;

    static CameraFollow _instance;
    public static CameraFollow Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
    }
    // Start is called before the first frame update
    public void Init(Transform followTarget)
    {
        _targetToFollow = followTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if(_targetToFollow != null)
        {
            transform.position = _targetToFollow.position + _offset;
        }
    }
}
