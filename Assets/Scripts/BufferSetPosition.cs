using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferSetPosition : MonoBehaviour
{
    #region public variables
    public Vector3 center; 
    #endregion
    void Start()
    {
        transform.position = center;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = center;
    }
}
