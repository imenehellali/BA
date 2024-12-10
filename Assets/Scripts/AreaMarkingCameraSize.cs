using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMarkingCameraSize : MonoBehaviour
{
    public Camera m_camera_2D;
    private Camera m_camera;
    void Start()
    {
        m_camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        m_camera.orthographicSize = m_camera_2D.orthographicSize + 200;
    }
}
