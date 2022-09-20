using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform targetToFollow;
    [SerializeField] private Vector3 offset;
    // Start is called before the first frame update
    public void Init(Transform followTarget)
    {
        targetToFollow = followTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if(targetToFollow != null)
        {
            transform.position = targetToFollow.position + offset;
        }
    }
}
