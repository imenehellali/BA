using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeAWalk : MonoBehaviour
{
    public Transform objectToMode;
    public Transform targetObject;
    [SerializeField]
    private float smoothSpeed=.125f;
    [SerializeField]
    private Vector3 offset;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPos = targetObject.position + offset;
        objectToMode.position =targetPos;
    }
}
