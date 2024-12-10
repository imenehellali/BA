using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Examples;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Examples;

public class TrailerBehavior : MonoBehaviour
{
    public GameObject cameraContainer=null;
    public GameObject lightContainer=null;

    private Camera m_camera_2D;
    private Light m_light;
    private float zoomSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        m_camera_2D = cameraContainer.GetComponent<Camera>();
        m_light = lightContainer.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_camera_2D.orthographicSize >= 100f)
        {
            m_light.enabled = false;
            StartCoroutine(WaitMoments());
        }
        else
        {
            m_camera_2D.orthographicSize += zoomSpeed*Time.deltaTime;
            m_light.enabled = true;
        }
    }

    IEnumerator WaitMoments()
    {
        yield return new WaitForSeconds(20f);
        m_camera_2D.orthographicSize = 20f;
    }
}
